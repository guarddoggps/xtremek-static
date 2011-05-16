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
using AlarmasABC.BLL.ProcessSecurityScheme;
using AlarmasABC.Core.Security;
using Telerik.Web.UI;

public partial class Security_SchemeEdit : System.Web.UI.UserControl
{
   	static DataSet _ds = new DataSet();

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if(!IsPostBack)
        { 
            
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack)
			{
                if (Session["_schemeID"] != null)
                {
					// Edit mode
                    string _ID = Session["_schemeID"].ToString();
                    loadSchemeInfo(_ID);
                    loadModule(_ID);
                    loadNotUserGroup(_ID);
                    loadUserGroup(_ID);
                    loadNotListedUser(_ID);// change on 3rd march
                    loadListedUser(_ID);
					Session["_schemeID"] = null;
                }
                else if (Session["Insert"] != null)
                {
					// Insert mode
                    loadModule();
					loadUser();
                    Session["Insert"] = null;
                    Session["IsInserted"] = 1;
                }
			}
        }
        catch (Exception ex)
        { 
        	Console.WriteLine(ex.Message);
        }
    }

    private void setHiddenvalue()
    {
        try
        {
            string _val = "";

            if (_lstBT.Items.Count > 0)
            {
                _val = _lstBT.Items[0].Value + ";";

                for (int i = 1; i < _lstBT.Items.Count; i++)
                {
                    _val += _lstBT.Items[i].Value + ";";
                }

                _hListValue.Value = _val;
            }
        }
        catch
        {

        }
    }

    private void loadSchemeInfo(string _schemeID)
    {
        DataSet _ds = new DataSet();

        ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        _processEditScheme.SchemeID = _schemeID;
		try
		{
		    _processEditScheme.getSchemeInfo();
		    _ds = _processEditScheme.DsSchemeInfoSelect;
		    if (_ds.Tables[0].Rows.Count > 0)
		    {
		        _txtSchemeName.Text = _ds.Tables[0].Rows[0]["schemeName"].ToString();
		        string _dfltScheme = _ds.Tables[0].Rows[0]["defaultScheme"].ToString();

		        if (_dfltScheme == "True")
		        {
		            _chkDefaultScheme.Checked = true;
		        }
		    }
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}

    }
    private void loadNotUserGroup(string _schemeID)
    {
		try
		{
		    DataSet _ds = new DataSet();
		    ProcessEditScheme _processEditScheme = new ProcessEditScheme();
		    _processEditScheme.SchemeID = _schemeID;
		    _processEditScheme.ComID = Session["trkCompany"].ToString();
		    _ds = _processEditScheme.getNotUserGroup();
		    _lstNBT.DataSource = _ds;
		    _lstNBT.DataTextField = "groupName";
		    _lstNBT.DataValueField = "groupID";
		    _lstNBT.DataBind();		
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}

        //ListedUpdate.Update();
        //NotListedUpdate.Update();

    }

    private void loadUserGroup(string _schemeID)
    {

		try
		{
        	DataSet _ds = new DataSet();

		    ProcessEditScheme _processEditScheme = new ProcessEditScheme();
		    _processEditScheme.SchemeID = _schemeID;
		    _processEditScheme.ComID = Session["trkCompany"].ToString();
		    _ds = _processEditScheme.getUserGroup();
		    _lstBT.DataSource = _ds;
		    _lstBT.DataTextField = "groupName";
		    _lstBT.DataValueField = "groupID";
		    _lstBT.DataBind();		
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}

        setHiddenvalue();
        //ListedUpdate.Update();
        //NotListedUpdate.Update();

    }

    private void loadNotListedUser(string _schemeID)
    {
		try
		{
		    DataSet _ds = new DataSet();

		    _lstNBT.Items.Clear();
		    ProcessEditScheme _processEditScheme = new ProcessEditScheme();
		    _processEditScheme.SchemeID = _schemeID;
		    _processEditScheme.ComID = Session["trkCompany"].ToString();
		    _ds = _processEditScheme.getNotListedUser();
		    _lstNBT.DataSource = _ds;

		    _lstNBT.DataTextField = "login";
		    _lstNBT.DataValueField = "uID";
		    _lstNBT.DataBind();		
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}
    }

    private void loadListedUser(string _schemeID)
    {
		try
		{
		    DataSet _ds = new DataSet();
		    _lstBT.Items.Clear();
		    ProcessEditScheme _processEditScheme = new ProcessEditScheme();
		    _processEditScheme.SchemeID = _schemeID;
		    _processEditScheme.ComID = Session["trkCompany"].ToString();
		    _ds = _processEditScheme.getListedUser();
		    _lstBT.DataSource = _ds;

		    _lstBT.DataTextField = "login";
		    _lstBT.DataValueField = "uID";
		    _lstBT.DataBind();
		    setHiddenvalue();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}
    }

    private void loadUserGroup()
    {
        ProcessNewScheme _proNewSchm = new ProcessNewScheme();
        try
        {
            _proNewSchm.ComID = int.Parse(Session["trkCompany"].ToString());
            _proNewSchm.fillUserGroup(_lstNBT);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            _proNewSchm = null;
        }

    }

    private void loadUser()
    {

        ProcessNewScheme _porcNewSckm = new ProcessNewScheme();
        try
        {
            _porcNewSckm.ComID = int.Parse(Session["trkCompany"].ToString());
            _porcNewSckm.fillUsers(_lstNBT);

        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

        finally
        {
            _porcNewSckm = null;
        }

    }

    private void loadModule()
    {

        ProcessNewScheme _ProNewSkm = new ProcessNewScheme();
        try
        {
            _ProNewSkm.loadModuleInfo();
            _ds = _ProNewSkm.Ds;
            Session["ModuleData"] = _ds;

            _grdModule.DataSource = _ds.Tables[0];
            _grdModule.DataBind();

            Session["ModuleData"] = _ds;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            _ProNewSkm = null;
        }
    }

    private void loadModule(string _schemeID)
    {
        DataSet _ds = new DataSet();

        ProcessEditScheme _processEditScheme = new ProcessEditScheme();
        _processEditScheme.SchemeID = _schemeID;
        _ds = _processEditScheme.getMoudule();
        Session["ModuleData"] = _ds;

        _grdModule.DataSource = _ds.Tables[0];
        _grdModule.DataBind();

        
    }

    protected void _grdModule_DataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList _dlstPages = (DataList)e.Item.FindControl("_dlstPages");
            DataSet _ds = new DataSet();
            _ds = (DataSet)Session["ModuleData"];
            DataView _dvPages = _ds.Tables[1].DefaultView;
            _dvPages.RowFilter = "moduleID=" + e.Item.Cells[0].Text;

            if (_dvPages.Count > 0)
            {
                _dlstPages.DataSource = _dvPages;
                _dlstPages.DataBind();
            }
            else
            {
                ((HtmlControl)e.Item.FindControl("_moduleHeader")).Visible = false;
            }
        }
    }

    private void clearControls()
    {
        string _ID = Session["_schemeID"].ToString();
        _lstNBT.Items.Clear();
        _lstBT.Items.Clear();
        loadModule(_ID);
        loadUserGroup(_ID);
        loadNotUserGroup(_ID);
    }

    protected void _rdoUserGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdoUserGroup.Checked)
        {
            
            if (Session["_ID"] != null)
            {
                string _ID = Session["_ID"].ToString();
                loadUserGroup(_ID);
                loadNotUserGroup(_ID);
            }
            else
                loadUserGroup();
        }
    }
    protected void _rdoUser_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdoUser.Checked)
        {
            if (Session["_ID"] != null)
            {
                string _ID = Session["_ID"].ToString();
                loadNotListedUser(_ID);
                loadListedUser(_ID);
            }
            else
                loadUser();
        }

    }

    protected void _dlstPages_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                string _insStatus = ((Label)e.Item.FindControl("_lblInsertStatus")).Text;
                if (_insStatus == "1")
                    ((CheckBox)e.Item.FindControl("_chkInsert")).Checked = true;

                string _viewStatus = ((Label)e.Item.FindControl("_lblViewStatus")).Text;
                if (_viewStatus == "1")
                    ((CheckBox)e.Item.FindControl("_chkView")).Checked = true;

                string _editStatus = ((Label)e.Item.FindControl("_lblEditStatus")).Text;
                if (_editStatus == "1")
                    ((CheckBox)e.Item.FindControl("_chkEdit")).Checked = true;

                string _deleteStatus = ((Label)e.Item.FindControl("_lblDeleteStatus")).Text;
                if (_deleteStatus == "1")
                    ((CheckBox)e.Item.FindControl("_chkDelete")).Checked = true;

                string _status = ((Label)e.Item.FindControl("_lblStatus")).Text;
                if (_status == "1")
                    ((CheckBox)e.Item.FindControl("_chkPage")).Checked = true;

            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    protected void _grdModule_UpdateCommand(object source, DataGridCommandEventArgs e)
    {

    }
    
}
