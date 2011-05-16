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
using AlarmasABC.Core.Admin;
using AlarmasABC.BLL.ProcessUserGroup;
using AlarmasABC.DAL.Queries;

public partial class CompanyAdmin_UserGroup : System.Web.UI.Page
{
    bool _isDelete = false;
    bool _isEdit = false;
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Session["RoleID"].ToString() == "1" || Session["RoleID"].ToString() == "2")
		{
			_isDelete = true;
            _isEdit = true;
		}

        if (!IsPostBack)
        {
            try
            {
                if (Session["RoleID"].ToString() != "1" && Session["RoleID"].ToString() != "2")
                {
                    DataSet _ds = new DataSet();
                    _ds = FormPermission.LoadPermission();
                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in _ds.Tables[0].Rows)
                        {
                            if (row["FormName"].ToString() == "User Group")
                            {
                                if (int.Parse(row["delete"].ToString()) != 0)
                                {
									_isDelete = true;
                                }
                                if (int.Parse(row["insert"].ToString()) == 0)
                                {
                                    btnAdd.Enabled = false;
                                }

                                if (int.Parse(row["edit"].ToString()) != 0)
                                {
                                    _isEdit = true;
                                }
                            }
                        }
                    }
                }

                try
                {
                    int _comID = int.Parse(Session["trkCompany"].ToString());
                    loadGrid(_comID);
                }
                catch (Exception ex)
                {

                }
            }

            catch (Exception ex)
            {

            }
            
        }
    }

    private void loadGrid(int _comID)
    {

        ProcessGroupCompanyWise _userGroup = new ProcessGroupCompanyWise();
        try
        {
            _userGroup.ComID = _comID;
            _userGroup.invoke();
            _rgridGroup.DataSource = _userGroup.Ds;
            _rgridGroup.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _userGroup = null;
        }
    }


    protected void _rgridGroup_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int _comID = int.Parse(Session["trkCompany"].ToString());
            loadGrid(_comID);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    protected void _grdgroup_Edit(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string typeID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["groupID"].ToString();

        try
        {
            _rgridGroup.MasterTableView.EditMode = GridEditMode.InPlace;
        }
        catch (Exception exp)
        {
            exp.Message.ToString();
        }
        finally
        {

        }
    }

    protected void _grdGroup_Update(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            string lblGrpName = ((Label)e.Item.FindControl("lblgrpName")).Text;
            if (lblGrpName == "Administrator")
            {
                e.Canceled = true;
                return;
            }
        }
        if (e.CommandName == "Update")
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string _groupID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["groupID"].ToString();
            string _groupName = (editedItem["groupName"].Controls[0] as TextBox).Text;

            UserGroup _userGroup = new UserGroup();
            ProcessUserGroup _updateGroup = new ProcessUserGroup(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
            try
            {
                _userGroup.GroupID = int.Parse(_groupID);
                _userGroup.GroupName = _groupName;

                _updateGroup.UserGroup = _userGroup;
                _updateGroup.invoke();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                _userGroup = null;
                _updateGroup = null;
            }
            _lblMessage.Text = "User Group Updated Successfully.";
            _lblMessage.ForeColor = System.Drawing.Color.Green;

        }
    }

    private void CanNotDelete()
    {
        _lblMessage.ForeColor = System.Drawing.Color.Red;
        _lblMessage.Text = "User exists under this group, can not be deleted";
    }
    protected void _rgridGroup_DeleteCommand(object source, GridCommandEventArgs e)
    {
        GridDataItem item = (GridDataItem)e.Item;
        string _groupID = item.OwnerTableView.DataKeyValues[item.ItemIndex]["groupID"].ToString();
        string _groupName = ((Label)e.Item.FindControl("lblgrpName")).Text;

        if (!UserExist(_groupID))
        {            
            UserGroup _userGrp = new UserGroup();
            ProcessUserGroup _delGroup = new ProcessUserGroup(AlarmasABC.BLL.InvokeOperations.operations.DELETE);
            try
            {
                _userGrp.GroupID = int.Parse(_groupID);
                _delGroup.UserGroup = _userGrp;
                _delGroup.invoke();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _delGroup = null;
                _userGrp = null;
            }
            clearControls();

            _lblMessage.Text = "User Group " + _groupName + " Deleted Successfully.";
            _lblMessage.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            _lblMessage.Text = "User Group \"" + _groupName + "\" is in use and can't be deleted.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
        }

    }

    private bool UserExist(string gID)
    {
        ExecuteSQL _execute = new ExecuteSQL();

        try
        {

            string _strSQL = "SELECT * FROM tblUser JOIN tblGroup ON" +
                             " (tblGroup.groupID = tblUser.groupID) WHERE tblGroup.groupID=" + gID + 
                             " AND coalesce(tblUser.isDelete,'0' ) != '1';";
            if (_execute.IsExistData(_strSQL))            
                return true;
            else
                return false;

        }
        catch (Exception exp)
        {
            exp.Message.ToString();
            return true;
        }
        finally
        {
            _execute = null;
        }

        return true;
    }
    protected void _rgridGroup_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem)
        {
            GridCommandItem commandItem = (GridCommandItem)e.Item;
            Button addNewRecord = (Button)(commandItem.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0]);
            addNewRecord.Visible = false;
        }

        foreach (GridColumn col in _rgridGroup.MasterTableView.RenderColumns)
        {
            if (col.UniqueName == "Edit")
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
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (isValidData())
            saveData();
    }

    private void saveData()
    {
        UserGroup _userGroup = new UserGroup();
        ProcessUserGroup _pGroup = new ProcessUserGroup(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
        try
        {
            int _comID =int.Parse(Session["trkCompany"].ToString());
            _userGroup.GroupName = _txtGroupName.Text;
            _userGroup.ComID = _comID;

            _pGroup.UserGroup = _userGroup;
            _pGroup.invoke();
            loadGrid(_comID);
            clearControls();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        finally
        {
            _userGroup = null;
            _pGroup = null;
        }
    }
    private bool isValidData()
    {
        ExecuteSQL _execute=new ExecuteSQL();
        try
        {
            string _comID = Session["trkCompany"].ToString();
            string _strSQL = "SELECT * FROM tblGroup WHERE groupName='" + _txtGroupName.Text + 
                             "' AND comID=" + _comID + "";

            if (_txtGroupName.Text == "")
            {
                _lblMessage.Text = "Please Enter Group Name";
                _lblMessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else if (_execute.IsExistData(_strSQL))
            {
                _lblMessage.Text = "Group Name Already Exists.";
                _lblMessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        finally
        {
            _execute = null;
        }
        
        

        return true;
    }

    private void clearControls()
    {
        
        _txtGroupName.Text = "";
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        _lblMessage.Text = "";
        _txtGroupName.Text = "";
        
    }
}
