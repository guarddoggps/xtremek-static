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
using AlarmasABC.BLL.ProcessRulesData;
using AlarmasABC.Core.Tracking;

public partial class CompanyAdmin_AssinRules : System.Web.UI.UserControl
{
    private DataSet _ds = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            chkAllUnit.Visible = false;
            loadRules();
            loadGeofence();
            BindParentGrid(Session["trkCompany"].ToString());
        }

        /*DataSet dataSet = new DataSet();
        dataSet = FormPermission.LoadPermission();

        if (dataSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                if (row["formName"].ToString() == "Rules")
                {
                    if (!((bool)row["delete"]))
                    {
                    }
                    if (!((bool)row["insert"]))
                    {
                        ApplyRules.Enabled = false;
                    }

                    if (!((bool)row["edit"]))
                    {


                    }
                }
            }
        }*/
    }

    private void loadGeofence()
    {
        string comID = Session["trkCompany"].ToString();
        ProcessRulesData.fillDropDownItems(_ddlGeofence, comID, "geo");
    }
    private void BindParentGrid(string comID)
    {
        try
        {
            ProcessRulesData _unitRules = new ProcessRulesData();
            RulesData _units = new RulesData();
            if (Session["roleID"] != null)
            {
                if (Session["roleID"].ToString() != "3")
                {
                    _units.ComID = int.Parse(comID);
                    _units.UID = int.Parse(Session["roleID"].ToString());

                    _unitRules.RulesData = _units;
                    _unitRules.GetUnits();
                    _ds = _unitRules.Ds;

                }
                else
                {
                    //RulesData _units = new RulesData();
                    _units.ComID = int.Parse(comID);
                    _unitRules.RulesData = _units;
                    _unitRules.GetUnits3();
                    _ds = _unitRules.Ds;
                    
                }
            }

            _ds.Tables[0].TableName = "Group";
            _ds.Tables[1].TableName = "Units";
            int count = _ds.Tables[0].Rows.Count;
            _grdMain.DataSource = _ds.Tables["Group"];
            _grdMain.DataBind();
            if (count > 1)
            {
                chkAllUnit.Visible = true;
            }
            else
            {
                chkAllUnit.Visible = false;
                ConfirmationFlagLabel.ForeColor = System.Drawing.Color.Red;
                ConfirmationFlagLabel.Text = "No Unit Record Found For This company";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void _grdMain_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataView _Units = _ds.Tables["Units"].DefaultView; 
            _Units.RowFilter = "typeID=" + e.Item.Cells[0].Text + " ";

            DataList lstUnit = (DataList)e.Item.FindControl("lstUnits");

            if (_Units.Count > 0)
            {
                lstUnit.DataSource = _Units;
                lstUnit.DataBind();
            }
            else
            {
                HtmlTable tbl = (HtmlTable)e.Item.FindControl("tblUnits");
                tbl.Visible = false;
            }

        }

    }



    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {

        foreach (DataGridItem item in _grdMain.Items)
        {
            CheckBox chkAll = (CheckBox)item.FindControl("chkAll");
            DataList lstUnit = (DataList)item.FindControl("lstUnits");

            if (chkAll.Checked)
            {
                foreach (DataListItem lstItem in lstUnit.Items)
                {
                    CheckBox chkUnit = (CheckBox)lstItem.FindControl("chkUnit");
                    chkUnit.Checked = true;
                }
            }
            else
            {
                foreach (DataListItem lstItem in lstUnit.Items)
                {
                    CheckBox chkUnit = (CheckBox)lstItem.FindControl("chkUnit");
                    chkUnit.Checked = false;
                }
            }
        }
        
        ConfirmationFlagLabel.Text = "";

    }


    protected void chkAllUnit_Checked(object sender, EventArgs e)
    {

        foreach (DataGridItem item in _grdMain.Items)
        {
            DataList lstUnit = (DataList)item.FindControl("lstUnits");

            if (chkAllUnit.Checked)
            {
                foreach (DataListItem lstItem in lstUnit.Items)
                {
                    CheckBox chkUnit = (CheckBox)lstItem.FindControl("chkUnit");
                    chkUnit.Checked = true;
                }
            }
            else
            {
                foreach (DataListItem lstItem in lstUnit.Items)
                {
                    CheckBox chkUnit = (CheckBox)lstItem.FindControl("chkUnit");
                    chkUnit.Checked = false;
                }
            }
        }
        ConfirmationFlagLabel.Text = "";

    }
    protected void _grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        foreach (DataGridItem item in _grdMain.Items)
        {
            CheckBox chkAll = (CheckBox)e.Item.FindControl("chkAll");
            if (chkAll.Checked)
            {
                DataList lstUnit = (DataList)item.FindControl("lstUnits");

                foreach (DataListItem lstItem in lstUnit.Items)
                {
                    CheckBox chkUnit = (CheckBox)lstItem.FindControl("chkUnit");
                    chkUnit.Checked = true;
                }
            }
        }
    }
   

    public void loadRules()
    {
        try
        {
            string comID = Session["trkCompany"].ToString();
            ProcessRulesData.fillDropDownItems(_ddlRules, comID, "rules");


        }
        catch (Exception ex)
        {

        }
        finally
        {

        }
    }

    protected void rulesDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
    private void createStr()
    {
       
        
       
        foreach (DataGridItem item in _grdMain.Items)
            {
            DataList lstUnit = (DataList)item.FindControl("lstUnits");


            foreach (DataListItem lstItem in lstUnit.Items)
            {
                CheckBox chkUnit = (CheckBox)lstItem.FindControl("chkUnit");
                if (chkUnit.Checked)
                {
                    Label UnitID = (Label)lstItem.FindControl("lblUnitID");
                     RulesData rules = new RulesData();
                     ProcessRulesData rulesObj = new ProcessRulesData();
                        rules.UnitID = int.Parse(UnitID.Text.Trim());
                        rules.RulesID = int.Parse(_ddlRules.SelectedValue.ToString()) ;
                        rules.GeoID = int.Parse(_ddlGeofence.SelectedValue.ToString());
                        rules.Email = Email.Text.ToString();
                        rules.Message = Subject.Text.ToString();
                        rules.IsActive = true;
                        rulesObj.RulesData = rules;
                        rulesObj.AssignRules();
                        ConfirmationFlagLabel.ForeColor = System.Drawing.Color.Green;
                        ConfirmationFlagLabel.Text = "Rules Applied Succesfully.";
                        //FormRest();
                        
                }
            }
         }
        //Database db = new Database();
        //bool flag = db.executeNonQry(insertSQl);
        //db.Close();
        //if (flag)
        //{
        //    ConfirmationFlagLabel.ForeColor = System.Drawing.Color.Green;
        //    ConfirmationFlagLabel.Text = "Rules Applied Succesfully.";
        //    FormRest();
        //    BindParentGrid(Session["trkCompany"].ToString());

        //}
        //else
        //{
        //    ConfirmationFlagLabel.ForeColor = System.Drawing.Color.Red;
        //    ConfirmationFlagLabel.Text = "Failed To Apply Rules!";
        //    FormRest();
        //}

    }
        
    protected void ApplyRules_Click1(object sender, EventArgs e)
    {
        //CreatePolice();
        createStr();
    }

    public void FormRest()
    {
        
        Email.Text = "";
        Subject.Text = "";
        PName.Text = "";

        _ddlRules.SelectedIndex = 0;
        _ddlGeofence.SelectedIndex = 0;

    }
    protected void rulesDropDownList_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ConfirmationFlagLabel.Text = "";
    }
}
