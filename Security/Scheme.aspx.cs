using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using AlarmasABC.Core.Security;
using AlarmasABC.BLL.ProcessSecurityScheme;
using AlarmasABC.DAL.Queries;

public partial class Security_Scheme : System.Web.UI.Page
{
    DataSet _ds = new DataSet();
    string _message = "";
	bool _isEdit = false;
	bool _isDelete = false;
	bool _isInsert = false;
    protected void Page_Load(object sender, EventArgs e)
    {
		try
        {
        	if (Session["RoleID"].ToString() != "1" && Session["RoleID"].ToString() != "2")
           	{
             	_ds = FormPermission.LoadPermission();
            	if (_ds.Tables[0].Rows.Count > 0)
             	{
                	foreach (DataRow row in _ds.Tables[0].Rows)
                   	{
                    	if (row["FormName"].ToString() == "Security Scheme")
                    	{
                       		if (int.Parse(row["delete"].ToString()) != 0)
                          	{
								_isDelete = true;
                           	}
                          	if (int.Parse(row["insert"].ToString()) != 0)
                         	{
                              	_isInsert = true;
                         	}

                          	if (int.Parse(row["edit"].ToString()) != 0)
                        	{
                           		_isEdit = true;
                          	}
                      	}
                  	}
              	}
           	}
			else
			{
				_isDelete = true;
                _isInsert = true;
                _isEdit = true;
			}
		}
        catch (Exception ex)
      	{
			Console.WriteLine(ex.Message);
      	}

        if (!IsPostBack)
        {
            Session["_ID"] = null;
            LoadSchemes();
        }
       
    }

    private void loadScheme()
    {

        try
        {
            ProcessScheme _porcsScheme = new ProcessScheme(AlarmasABC.BLL.InvokeOperations.operations.SELECT);
            _porcsScheme.ComID = 1;
            _porcsScheme.invoke();
            DataSet _ds = new DataSet();
            _ds = _porcsScheme.Ds;

            _grdScheme.DataSource = _ds;
            _grdScheme.DataBind();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {

        }

    }

    private void LoadSchemes()
    {
        try
        {
            string _comID = Session["trkCompany"].ToString();

            string _strSQL = "SELECT * FROM tblSecurityScheme WHERE comID = " + _comID + "; ";
            _strSQL += " SELECT * FROM tblSecurityScheme WHERE comID = " + _comID + " AND defaultScheme = 1 ;";
            _strSQL += " SELECT s.ID,f.formName,s.schemeName,p.fullaccess,p.delete,p.view,p.insert,p.edit";
            _strSQL += " FROM tblSchemePermission p INNER JOIN tblSecurityScheme s ON p.schemeID = s.ID";
            _strSQL += " INNER JOIN tblForms f ON f.ID = p.formID ORDER BY s.ID;";

            ExecuteSQL _ex = new ExecuteSQL();
            DataSet _ds = new DataSet();

            _ds = _ex.getDataSet(_strSQL);

            _grdScheme.DataSource = _ds;
            _grdScheme.DataBind();
            
        }
        catch (Exception ex)
        { 
        	Console.WriteLine(ex.Message);
        }
    }

    protected void _grdScheme_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            
            if (e.CommandName == "Delete")
            {
                string _schemeID = ((Label)e.Item.FindControl("lblSchemeID")).Text;
                string _schemeName = ((Label)e.Item.FindControl("lblSchemeName")).Text;
                
                Session["_schemeID"] = _schemeID;
                Session["_ID"] = _schemeID;
                if (!UserExist(_schemeID))
                {
                    deleteScheme();
                }
                else
                {
                    
                    _message = "Scheme " + _schemeName + " is in use,can't be delete";
                    _lblMsg.ForeColor = System.Drawing.Color.Red;
                    _lblMsg.Text = _message;

                }

            }
            else if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)_grdScheme.MasterTableView.GetColumn("EditCommandColumn");
                editColumn.Visible = true;
                Session["Insert"] = "Insert";
                Session["IsInserted"] = "yes";

            }
            else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
            {
                e.Canceled = true;
            }
            else
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)_grdScheme.MasterTableView.GetColumn("EditCommandColumn");
                if (!editColumn.Visible)
                    editColumn.Visible = true;
                Session["IsInserted"] = null;

            }

            if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            {
                string _schemeID = ((Label)e.Item.FindControl("lblSchemeID")).Text;
                Session["_schemeID"] = _schemeID;
                Session["_ID"] = _schemeID;

            }
           
            LoadSchemes();
             
            // _grdScheme.Rebind();
        }
        catch (Exception ex)
        { 
        	Console.WriteLine(ex.Message);
        }
    }


    private bool UserExist(string sID)
    {
        ExecuteSQL _execute = new ExecuteSQL();

        try
        {

            string _strSQL = "SELECT * from tblUserWiseScheme WHERE schemeID = " + sID + ";";
            if (_execute.IsExistData(_strSQL))
                return true;
            else
                return false;

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return true;
        }
        finally
        {
            _execute = null;
        }

        return true;
    }

    protected void _grdScheme_ItemBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            string _status = e.Item.Cells[1].Text;
            DataSet _ds = new DataSet();
            if (_status == "True")
            {
                ((CheckBox)e.Item.FindControl("_chkDefault")).Checked = true;
            }

            ProcessScheme _procSchm = new ProcessScheme();
            try
            {

                _procSchm.ID = int.Parse(((Label)e.Item.FindControl("lblSchemeID")).Text);
                _procSchm.selectSchemeUsers();
                _ds = _procSchm.Ds;

                ((Label)e.Item.FindControl("_lblUserCount")).Text = _ds.Tables[0].Rows[0]["UserCount"].ToString();

                _procSchm.selectSchemeUnits();
                _ds = _procSchm.Ds;

                ((Label)e.Item.FindControl("_lblUnitCount")).Text = _ds.Tables[0].Rows[0]["unitCount"].ToString();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                _procSchm = null;
                _ds = null;
                
                
            }

        }

		foreach (GridCommandItem cmdItm in _grdScheme.MasterTableView.GetItems(GridItemType.CommandItem)) 
       	{ 
		    LinkButton Addbtn = (LinkButton)cmdItm.FindControl("InitInsertButton"); 
			if (!_isInsert)
			{
		      	Addbtn.Visible = false; 
		      	Button btn = (Button)cmdItm.FindControl("AddNewRecordButton"); 
		      	btn.Visible = false; 
			}
			else
			{
				Addbtn.Text = " Add new scheme";
			}
      	} 

        foreach (GridColumn col in _grdScheme.MasterTableView.RenderColumns)
        {
            if (col.UniqueName == "EditCommandColumn")
            {
					if (_isEdit)
                     	col.Visible = true;
					else
                       	col.Visible = false;
            }

            if (col.UniqueName == "Delete")
            {
					if (_isDelete)
                     	col.Visible = true;
					else
                       	col.Visible = false;
            }
        }
        _lblMsg.Text = _message;
    }
    protected void _grdScheme_UpdateCommand(object source, GridCommandEventArgs e)
    {
		
        //if (e.CommandName == "Update")
        //{
            GridEditableItem editedItem = e.Item as GridEditableItem;
            UserControl userControl = (UserControl)e.Item.FindControl(GridEditFormItem.EditFormUserControlID);
            string sName = (userControl.FindControl("_txtSchemeName") as TextBox).Text;
            bool dScheme = (userControl.FindControl("_chkDefaultScheme") as CheckBox).Checked;
            bool rdoUser = (userControl.FindControl("_rdoUser") as CheckBox).Checked;
            bool rdoUserGroup = (userControl.FindControl("_rdoUserGroup") as CheckBox).Checked;
            ListBox lstNBT = (userControl.FindControl("_lstNBT") as ListBox);
            ListBox lstBT = (userControl.FindControl("_lstBT") as ListBox);
            HiddenField hListValue = (userControl.FindControl("_hListValue") as HiddenField);
            DataGrid grid = (userControl.FindControl("_grdModule") as DataGrid);

            //Load();
        

            if (sName != null || sName != "")
            {
                if (Session["IsInserted"] == null)
                {
                    if (e.CommandName == "Update" && Session["IsInserted"] == null)
                    {
                        //deleteScheme();
                    }
                    else
                    {
                        Session["IsInserted"] = null;
                    }
                }
                else
                {
                    Session["IsInserted"] = null;
                }
                int t = saveSchemeInfo(dScheme, sName);
                if (t == 0)
                    if (saveScheme(rdoUser, rdoUserGroup, hListValue, grid, lstBT))
                    {
                        clearControls(lstNBT, lstBT);
                        Session["IsInserted"] = null;
                        //_lblMessage.Text = " Security Scheme Updated successfully.";
                        //_lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        //_lblMessage.Text = " Failed !! Try Again .";
                        //_lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                else if (t == -3)
                {
                    //_lblMessage.Text = " Security Scheme Already Exists.";
                    // _lblMessage.ForeColor = System.Drawing.Color.Green;
                }
            }
        //}
        //else
        //{
        //    deleteScheme();
            LoadSchemes();
            
        //}	
    }

    //private bool isValid()
    //{
    //    if (_txtSchemeName.Text == "")
    //    {
    //        _lblMessage.Text = "Please Enter Scheme Name..";
    //        _lblMessage.ForeColor = System.Drawing.Color.Red;
    //        return false;
    //    }

    //    return true;
    //}

    private void deleteScheme()
    {

        ProcessEditScheme _editScheme = new ProcessEditScheme();
        try
        {
            _editScheme.SId = int.Parse(Session["_ID"].ToString());
            _editScheme.CompanyId = int.Parse(Session["trkCompany"].ToString());
            _editScheme.deleteSchemeInfo();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _editScheme = null;
        }

    }

    private int saveSchemeInfo(bool _chkDefaultScheme, string _txtSchemeName)
    {
        string _comID = Session["trkCompany"].ToString();
        bool _defaultScheme = _chkDefaultScheme;

        ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        if (Session["IsInserted"] != null) {
			_processEditScheme.SchemeID = Session["_ID"].ToString();
		}
        _processEditScheme.SchemeName = _txtSchemeName;
        _processEditScheme.ComID = _comID;
        _processEditScheme.DefaultScheme = _defaultScheme;
        return _processEditScheme.saveSchemeInfo();

    }

    private bool saveScheme(bool _rdoUser, bool _rdoUserGroup, HiddenField _hListValue,DataGrid _grdScheme,ListBox _lst)
    {
        if (_rdoUser)
        {
            if (_hListValue.Value != "-1")
            {
                saveUserWiseScheme(_hListValue, _lst);
            }
            return saveSchemePermission(_grdScheme);
        }


        else if (_rdoUserGroup)
        {
            if (_hListValue.Value != "-1")
            {
                saveGroupWiseScheme(_hListValue, _lst);
            }
            return saveSchemePermission(_grdScheme);
        }


        return false;
    }

    private void clearControls(ListBox _lstNBT,ListBox _lstBT)
    {
        string _ID;
        if (Session["_ID"] != null)
        {
            _ID = Session["_ID"].ToString();
            _lstNBT.Items.Clear();
            _lstBT.Items.Clear();
            loadModule(_ID);
            loadUserGroup(_ID, _lstBT);
            loadNotUserGroup(_ID, _lstNBT);
        }//Session["_ID"] = null;
    }

    private bool saveUserWiseScheme(HiddenField _hListValue,ListBox lst)
    {

        string[] _groupList = (_hListValue.Value).Split(';');
        string _comID = Session["trkCompany"].ToString();
        int status = -1;


        if (_groupList.Length < 2)
        {

            for (int i = 0; i < lst.Items.Count; i++)
            {
                ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        		if (Session["IsInserted"] == null) 
				{
					_processEditScheme.SchemeID = Session["_ID"].ToString();
				}
                _processEditScheme.UserID = int.Parse(lst.Items[i].Value);
                _processEditScheme.ComID = _comID;
                status = _processEditScheme.saveUserWiseScheme();

            }
        }
        else
        {
            for (int i = 0; i < _groupList.Length - 1; i++)
            {

                if (_groupList[i] != "")
                {
                    ProcessEditScheme _processEditScheme = new ProcessEditScheme();
		    		if (Session["IsInserted"] == null) 
					{
						_processEditScheme.SchemeID = Session["_ID"].ToString();
					}
                    _processEditScheme.UserID = int.Parse(_groupList[i]);
                    _processEditScheme.ComID = _comID;
                    status = _processEditScheme.saveUserWiseScheme();
                }
            }
        }
        

        if (status == 0)
            return true;
        else
            return false;
    }

    private bool saveSchemePermission(DataGrid _grdScheme)
    {
        string _comID = Session["trkCompany"].ToString();

        foreach (DataGridItem item in _grdScheme.Items)
        {
            DataList _dlstPages = (DataList)item.FindControl("_dlstPages");
            foreach (DataListItem e in _dlstPages.Items)
            {
          		int _pageID = int.Parse(((Label)e.FindControl("_lblPageID")).Text);
              	bool _insert = ((CheckBox)e.FindControl("_chkInsert")).Checked;
               	bool _view = ((CheckBox)e.FindControl("_chkView")).Checked;
                bool _edit = ((CheckBox)e.FindControl("_chkEdit")).Checked;
                bool _delete = ((CheckBox)e.FindControl("_chkDelete")).Checked;
                bool _access = ((CheckBox)e.FindControl("_chkPage")).Checked;

                ProcessEditScheme _processEditScheme = new ProcessEditScheme();
               	SchemePermission _SchemePermission = new SchemePermission();

                _SchemePermission.ComID = int.Parse(_comID);
        		if (Session["IsInserted"] == null) 
				{
					_SchemePermission.SchemeID = int.Parse(Session["_ID"].ToString());
				}
                _SchemePermission.Delete = _delete;
                _SchemePermission.Edit = _edit;
                _SchemePermission.FormID = _pageID;
                _SchemePermission.FullAccess = _access;
                _SchemePermission.View = _view;
                _SchemePermission.Insert = _insert;

                _processEditScheme.SchemePermission = _SchemePermission;
                int status = _processEditScheme.saveSchemePermission();
            }
        }
        return true;
    }

    private bool saveGroupWiseScheme(HiddenField _hListValue, ListBox lst)
    {

        string[] _groupList = (_hListValue.Value).Split(';');
        string _comID = Session["trkCompany"].ToString();
        int status = -1;


        if (_groupList.Length < 2)
        {

            for (int i = 0; i < lst.Items.Count; i++)
            {

                ProcessEditScheme _processEditScheme = new ProcessEditScheme();
                _processEditScheme.GroupID = int.Parse(lst.Items[i].Value);
                _processEditScheme.ComID = _comID;
                status = _processEditScheme.saveGroupWiseScheme();

            }
        }
        else
        {
            for (int i = 0; i < _groupList.Length - 1; i++)
            {

                if (_groupList[i] != "")
                {
                    ProcessEditScheme _processEditScheme = new ProcessEditScheme();
                    _processEditScheme.GroupID = int.Parse(_groupList[i]);
                    _processEditScheme.ComID = _comID;
                    status = _processEditScheme.saveGroupWiseScheme();
                }

            }
        }
                

        if (status == 0)
            return true;
        else
            return false;
    }

    private void loadModule(string _schemeID)
    {

        ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        _processEditScheme.SchemeID = _schemeID;
        _ds = _processEditScheme.getMoudule();

        _grdScheme.DataSource = _ds.Tables[0];
        //_grdScheme.DataBind();
        LoadSchemes();
    }

    private void loadUserGroup(string _schemeID, ListBox _lstBT)
    {

        ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        _processEditScheme.SchemeID = _schemeID;
        _processEditScheme.ComID = Session["trkCompany"].ToString();
        _ds = _processEditScheme.getUserGroup();
        _lstBT.DataSource = _ds;
        _lstBT.DataTextField = "groupName";
        _lstBT.DataValueField = "groupID";
        _lstBT.DataBind();

    }

    private void loadNotUserGroup(string _schemeID, ListBox _lstNBT)
    {

        ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        _processEditScheme.SchemeID = _schemeID;
        _processEditScheme.ComID = Session["trkCompany"].ToString();
        _ds = _processEditScheme.getNotUserGroup();
        _lstNBT.DataSource = _ds;
        _lstNBT.DataTextField = "groupName";
        _lstNBT.DataValueField = "groupID";
        _lstNBT.DataBind();

    }
}
