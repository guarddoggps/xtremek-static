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
using AlarmasABC.Core.Admin;
using AlarmasABC.BLL.ProcessUnitType;
using AlarmasABC.BLL.ProcessCompany;
using Telerik.Web.UI;
using AlarmasABC.DAL.Queries;

public partial class UnitType : System.Web.UI.Page
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
                            if (row["FormName"].ToString() == "Unit Category")
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
            }

            catch (Exception ex)
            {

            }

        }
    }
    
   

    private void LoadUnitType()
    {
        try
        {
            ProcessCompanyUnitTypeQueries _unitType = new ProcessCompanyUnitTypeQueries();

            AlarmasABC.Core.Admin.UnitType _unit= new AlarmasABC.Core.Admin.UnitType();
            _unit.ComID = int.Parse(Session["trkCompany"].ToString());
            _unitType.UnitType = _unit;
            _unitType.invoke();
            DataSet _ds = new DataSet();
            _ds = _unitType.Ds;

            int count = _ds.Tables[0].Rows.Count;
            _rgrdtype.DataSource = _ds;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {

        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string _sql = "SELECT * FROM tblUnitType WHERE comID = " + Session["trkCompany"].ToString() + 
                      " AND typeName = '"+_txtTypeName.Text.ToString()+"';";
        ExecuteSQL haveData = new ExecuteSQL();
        if (!haveData.IsExistData(_sql))
        {
            if (_txtTypeName.Text.Length > 0)
            {
                SaveUnitType();
            }
        }
        else
        {
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Unit type " + _txtTypeName.Text.ToString() + " already exist for this company ";
        }
    }

    private void SaveUnitType()
    {
        AlarmasABC.Core.Admin.UnitType _unitType = new AlarmasABC.Core.Admin.UnitType();

        ProcessUnitTypeNotQueries insert = new ProcessUnitTypeNotQueries(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

        try
        {
            _unitType.TypeName = _txtTypeName.Text;
            string _comID = Session["trkCompany"].ToString();
            _unitType.ComID = int.Parse(_comID);

            insert.UnitType = _unitType;
            insert.invoke();

            clearControls();
            _lblMessage.ForeColor = System.Drawing.Color.Green;
            _lblMessage.Text = "Unit Type created Successfully.";
            

        }
        catch (Exception ex)
        {
            throw new Exception(" CompanyAdmin::UnitType:: " + ex.Message.ToString());
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Unit Type create fail.";
        }
        finally
        {
            _unitType = null;
        }

    }

    protected void _rgrdtype_UpdateCommand(object source, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string _typeID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["typeID"].ToString();
        string _typeName = (editedItem["typeName"].Controls[0] as TextBox).Text;

        AlarmasABC.Core.Admin.UnitType _unitType = new AlarmasABC.Core.Admin.UnitType();
        ProcessUnitTypeNotQueries _updateType = new ProcessUnitTypeNotQueries(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
        
        try
        {
            _unitType.Name = _typeName;
            _unitType.TypeID = int.Parse(_typeID);
            _unitType.ComID = int.Parse(Session["trkCompany"].ToString());

            _updateType.UnitType = _unitType;
            _updateType.invoke();

            
        }
        catch (Exception ex)
        {
            throw new Exception(" CompanyAdmin::UnitType:: " + ex.Message.ToString());
        }

        finally
        {
            _unitType = null;
            _updateType = null;
        }
        _lblMessage.Text = "Unit Type Updated Successfully.";
        _lblMessage.ForeColor = System.Drawing.Color.Green;


    }
    protected void _rgrdtype_DeleteCommand(object source, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;
        string _typeID = editedItem.OwnerTableView.DataKeyValues[editedItem.ItemIndex]["typeID"].ToString();
        string _tName = ((Label)e.Item.FindControl("lbltypeName")).Text;
        if (!UnitExist(_typeID))
        {
            ProcessUnitTypeNotQueries delete = new ProcessUnitTypeNotQueries(AlarmasABC.BLL.InvokeOperations.operations.DELETE);

            AlarmasABC.Core.Admin.UnitType _unitType = new AlarmasABC.Core.Admin.UnitType();
            try
            {

                _unitType.TypeID = int.Parse(_typeID);

                delete.UnitType = _unitType;
                delete.invoke();


            }
            catch (Exception ex)
            {
                throw new Exception(" :: " + ex.Message);
            }

            finally
            {
                _unitType = null;
            }
        }
        else
        {
            _lblMessage.Text = "Unit category "+_tName+ " in use and can't be deleted";
        }
    }

    private bool UnitExist(string uID)
    {
        ExecuteSQL _execute = new ExecuteSQL();

        try
        {

            string _strSQL = "SELECT * FROM tblUnits JOIN tblUnitType" +
                             " ON (tblUnits.typeID = tblUnitType.typeID)" +
                             " WHERE tblUnits.typeID = " + uID + 
                             " AND coalesce(tblUnits.isDelete,'0' ) != '1';";
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
    protected void _rgrdtype_EditCommand(object source, GridCommandEventArgs e)
    {

    }


    protected void _ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void _rgrdtype_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        LoadUnitType();
    }

    private void clearControls()
    {
        _txtTypeName.Text = "";
         
    }
    protected void _rgrdtype_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridCommandItem)
        {
            GridCommandItem commandItem = (GridCommandItem)e.Item;
            Button addNewRecord = (Button)(commandItem.Controls[0].Controls[0].Controls[0].Controls[0].Controls[0]);
            addNewRecord.Visible = false;
            e.Item.Edit = false;
        }

        foreach (GridColumn col in _rgrdtype.MasterTableView.RenderColumns)
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

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearField();
    }

    public void ClearField()
    {
        _txtTypeName.Text = "";
    }
}
