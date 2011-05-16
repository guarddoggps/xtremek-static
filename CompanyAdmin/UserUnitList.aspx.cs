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
using AlarmasABC.BLL.ProcessCompany;
using AlarmasABC.DAL.Queries;
using AlarmasABC.BLL.ProcessUser;
using AlarmasABC.BLL.ProcessUnit;

public partial class CompanyAdmin_UserUnitList : System.Web.UI.Page
{
    bool _isDelete = true;
    bool _isEdit = true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            _rdoUnit.Checked = true;
        }
        PageAccess();
            
    }


    private void PageAccess()
    {
        try
        {
            DataSet _ds = new DataSet();
            _ds = FormPermission.LoadPermission();
            if (_ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in _ds.Tables[0].Rows)
                {
                    if (row["FormName"].ToString() == "User & Unit List")
                    {
                            if (int.Parse(row["delete"].ToString()) == 0)
                            {
                                _isDelete = false;
                            }

                            if (int.Parse(row["insert"].ToString()) == 0)
                            {
                                _isEdit = false;
                            }
                    }
                }
            }
        }
        catch (Exception ex)
        {
			Console.WriteLine(ex.Message);
            _lblMessage.Text = "Page load without access rights";
        }
    }
    private void LoadUser(string _comID)
    {
        string _strSQL = "SELECT uID AS id,login,userName,groupName,email," +
                         " CAST((CASE u.isActive WHEN '1' THEN '1' WHEN '0' THEN '0' ELSE '0' END) AS BOOLEAN) AS status" +
                         " FROM tblUser u INNER JOIN tblGroup gr ON u.groupID=gr.groupID" +
                         " WHERE coalesce(u.isDelete,'0') != '1' AND u.comID = " + _comID + ";";

        ExecuteSQL _exc = new ExecuteSQL();

        _grdUser.DataSource = _exc.getDataSet(_strSQL);

        //_grdUser.DataBind();
    }

    private void LoadUnits(string _comID)
    {
        string _strSQL = "SELECT unitID AS id,unitName,t.typeName," +
                         " CAST((CASE u.isActive WHEN '1' THEN '1' WHEN '0' THEN '0' ELSE '0' END) AS BOOLEAN) AS status" +
                         " FROM tblUnits u INNER JOIN tblUnitType t ON t.typeID = u.typeID" +
                         " WHERE coalesce(u.isDelete,'0') != '1' AND u.comID = " + _comID + ";";

        ExecuteSQL _exc = new ExecuteSQL();

        _grdUnit.DataSource = _exc.getDataSet(_strSQL);
        //_grdUnit.DataBind();
    }

    protected void _grdUserUnit_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string _ID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();

        if (_rdoUnit.Checked)
            DeleteUnit(_ID);
        else if (_rdoUser.Checked)
            DeleteUser(_ID);

    }

    protected void _grdUserUnit_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //GridButtonColumn btn = (GridButtonColumn)_grdUnit.MasterTableView.Columns.FindByUniqueName("Disable");
        
        if (e.Item is GridCommandItem)
        {
            GridCommandItem commandItem = (GridCommandItem)e.Item;
            Button addNewRecord = (Button)(commandItem.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0]);
            addNewRecord.Visible = false;
            
        }
            if (e.Item is GridDataItem)
            {

                //GridDataItem dataItem = e.Item as GridDataItem;

                //CheckBox box = dataItem["status"].Controls[0] as CheckBox;
                
                GridDataItem item = e.Item as GridDataItem;
                if (item != null)
                {
                    LinkButton button = item["Disable"].Controls[0] as LinkButton;
                    if (_rdoUser.Checked)
                    {
                        CheckBox box;
                        box = (CheckBox)e.Item.FindControl("Status_User");

                        button.Visible = true;
                        if (!box.Checked)
                        {

                            button.Text = "Enable";
                        }
                        else
                        {

                            button.Text = "Disable";
                        }
                    }
                    else if (_rdoUnit.Checked)
                    {
                        CheckBox box;
                        box = (CheckBox)e.Item.FindControl("Status");

                        button.Visible = true;
                        if (!box.Checked)
                        {
                            
                            button.Text = "Enable";
                        }
                        else
                        {
                            
                            button.Text = "Disable";
                        }
                    }

                }



            }


        if (_rdoUser.Checked)
        {
            foreach (GridColumn col in _grdUser.MasterTableView.RenderColumns)
            {                
                if (col.UniqueName == "Disable")
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
        else
        {
            foreach (GridColumn col in _grdUnit.MasterTableView.RenderColumns)
            {
                if (col.UniqueName == "Disable")
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

    }

    private void DeleteUser(string ID)
    {
        ProcessUser _user = new ProcessUser(AlarmasABC.BLL.InvokeOperations.operations.DELETE);
        try
        {
            _user.UID = int.Parse(ID);
            _user.invoke();
            LoadUser(Session["trkCompany"].ToString());
            _lblMessage.Text = "User Deleted.";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _user = null;
        }
    }

    private void DeleteUnit(string UnitID)
    {
        ProcessUnitTypeNotQueries _unit = new ProcessUnitTypeNotQueries(AlarmasABC.BLL.InvokeOperations.operations.DELETE);

        try
        {
            _unit.UnitID = int.Parse(UnitID);
            _unit.invoke();
            LoadUnits(Session["trkCompany"].ToString());
            _lblMessage.Text = "Unit Deleted.";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _unit = null;
        }
    }

   

    protected void _DataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (_rdoUser.Checked)
        {
            LoadUser(Session["trkCompany"].ToString());
        }
        else if (_rdoUnit.Checked)
        {
            LoadUnits(Session["trkCompany"].ToString());     
        }
    }

    protected void _rdoUser_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdoUser.Checked) {
            _lblMessage.Text = "";
            //PageAccess();
            LoadUser(Session["trkCompany"].ToString());
            _grdUser.Visible = true;
            _grdUnit.Visible = false;
            _grdUser.Rebind();
        }
    }
    protected void _rdoUnit_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdoUnit.Checked) {
            _lblMessage.Text = "";
            //PageAccess();
            LoadUnits(Session["trkCompany"].ToString());
            _grdUnit.Visible = true;
            _grdUser.Visible = false;
            _grdUnit.Rebind();
        }
    }

    //protected void _grdUnit_EditCommand(object source, GridCommandEventArgs e)
    //{
    //    _lblMessage.Text = "Oklllllllllll";
    //}
   
    protected void _grdUserUnit_ItemUpdated(object source, GridUpdatedEventArgs e)
    {

    }
    protected void _grdUserUnit_ItemCommand(object source, GridCommandEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            string _ID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["ID"].ToString();
            GridDataItem item = e.Item as GridDataItem;
            if (item != null)
            {
                LinkButton button = item["Disable"].Controls[0] as LinkButton;

                if (button.Text == "Enable")
                {
                    if (_rdoUnit.Checked)
                        EnableUnit(_ID);
                }
                else if (button.Text == "Disable")
                {
                    if (_rdoUnit.Checked)
                        DisableUnit(_ID);
                    //else if (_rdoUser.Checked)
                    //DisableUser(_ID);
                }
            }
        }
    }

    private void DisableUser(string ID)
    {

        ProcessUser _user = new ProcessUser();
        try
        {
            _user.UID = int.Parse(ID);
            _user.disableUser();
            LoadUser(Session["trkCompany"].ToString());
            _lblMessage.Text = "Disable completed";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _user = null;
        }
    }

    private void DisableUnit(string UnitID)
    {
        ProcessUnitTypeNotQueries _unit = new ProcessUnitTypeNotQueries();

        try
        {
            _unit.UnitID = int.Parse(UnitID);
            _unit.disableUnit();
            LoadUnits(Session["trkCompany"].ToString());
            _grdUnit.DataBind();
            _lblMessage.Text = "Unit has been disabled.";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _unit = null;
        }
    }

    private void EnableUnit (string UnitID)
    {
        ProcessUnitTypeNotQueries _unit = new ProcessUnitTypeNotQueries();

        try
        {
            _unit.UnitID = int.Parse(UnitID);
            _unit.enableUnit();
            LoadUnits(Session["trkCompany"].ToString());
            _grdUnit.DataBind();
            _lblMessage.Text = "Unit has been enabled.";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _unit = null;
        }
    }
}
