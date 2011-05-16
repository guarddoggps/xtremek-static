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

public partial class CompanyAdmin_EditRules : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            LoadRules();
            loadGeofence();
          
        }
        try
        {
            DataSet _ds = new DataSet();
            _ds = FormPermission.LoadPermission();
            if (_ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in _ds.Tables[0].Rows)
                {
                    if (row["FormName"].ToString() == "Rules")
                    {
                        if (!((bool)row["delete"]))
                        {
                        }
                        if (!((bool)row["insert"]))
                        {

                        }

                        if (!((bool)row["Edit"]))
                        {
                            EditButton.Enabled = false;

                        }



                    }
                }
            }
            
        }
        catch (Exception ex)
        {
          
        }

    }



    private void LoadRules()
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



    private void LoadUnits(string  RulesID)
    {
       
        try
        {
           
            ProcessRulesData.fillDropDownItems(_ddlUnits, RulesID, "units");
            //ProcessRulesData _unitRules = new ProcessRulesData();
            //RulesData _units = new RulesData();

            //_units.RulesID = int.Parse(_ddlRules.SelectedValue.ToString());

            //_unitRules.RulesData = _units;

            //_unitRules.GetAssignedUnits();
            //DataSet _ds = _unitRules.Ds;
            //CancelUnitDataList.DataSource = _ds;
            //CancelUnitDataList.DataBind();



        }
        catch (Exception ex)
        {
        }
        finally
        {
        }

    }

    protected void _ddlRules_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_ddlRules.SelectedIndex > 0)
        {
            
            LoadUnits(_ddlRules.SelectedValue.ToString());
            //LoadRulesSubject();
        }

    }

    private void loadGeofence()
    {
        string comID = Session["trkCompany"].ToString();
        ProcessRulesData.fillDropDownItems(_ddlGeofence, comID, "geo");
    }

    private void LoadUnitRulesInfo()
    {
        try
        {
            ProcessRulesData _unitRules = new ProcessRulesData();
            RulesData _units = new RulesData();

            _units.UnitID = int.Parse(_ddlUnits.SelectedValue.ToString());

            _unitRules.RulesData = _units;

            _unitRules.UnitRuleInfo();
            DataSet _ds = _unitRules.Ds;
            EmailTextBox.Text = _ds.Tables[0].Rows[0][0].ToString();
            SubjectTextBox.Text = _ds.Tables[0].Rows[0][1].ToString();
            DescriptionTextBox.Text = _ds.Tables[0].Rows[0][2].ToString();
            isActiveCheckBox.Checked = bool.Parse(_ds.Tables[0].Rows[0][3].ToString());
            _ddlGeofence.SelectedValue = _ds.Tables[0].Rows[0][4].ToString();
        }
        catch (Exception ex)
        {
        }
    }

    private void LoadRulesSubject()
    {
    //    Database db = new Database();
    //    try
    //    {
    //        String sqlCommand = "select id, subject from tblunitwiserules where unitID in (select tblunitwiseRules.unitID from tblunitwiseRules join tblunits on (tblunitwiseRules.unitID=tblunits.unitID)where RulesID =" + _ddlRules.SelectedValue.ToString() + " and comid=" + Session["trkCompany"].ToString() + ")";
    //        db.fillCombo(RulesSubjectDropDownList, "Rules Subject", sqlCommand);
    //    }
    //    catch (SqlException ex)
    //    {
    //    }
    //    finally
    //    {
    //        db.Close();
    //    }

    }

    private void EditRules()
    {
        //string updateSQl = "";
        bool flag = false;
        //bool Status = cancelStatus;
        bool isActive = isActiveCheckBox.Checked;
      

        //foreach (DataListItem item in CancelUnitDataList.Items)
        //{
        //    CheckBox chkUnit = (CheckBox)item.FindControl("UnitCheckBox");

            if (_ddlUnits.SelectedIndex > 0)
            {
                //Label UnitID = (Label)item.FindControl("lblUnitID");
                RulesData rules = new RulesData();
                ProcessRulesData rulesObj = new ProcessRulesData();
                rules.UnitID = int.Parse(_ddlUnits.SelectedValue.ToString());
                rules.RulesID = int.Parse(_ddlRules.SelectedValue.ToString());
                rules.GeoID = int.Parse(_ddlGeofence.SelectedValue.ToString());
                rules.Email = EmailTextBox.Text.ToString();
                rules.Message = SubjectTextBox.Text.ToString();
                rules.IsActive = isActive;
                rulesObj.RulesData = rules;
                rulesObj.UpdateRules();
                flag = true;
                //updateSQl += " Update tblUnitWiseRules set RulesID=" + _ddlRules.SelectedValue.ToString() + ",GeofenceID=" + _ddlGeofence.SelectedValue.ToString() + ",Email='" + EmailTextBox.Text.Trim() + "',Subject='" + SubjectTextBox.Text.Trim() + "',Description='" + DescriptionTextBox.Text.Trim() + "',isActive=" + isActive + " where UnitID=" + UnitID.Text.Trim() + ";";
            }
        
       
        if (flag )
        {
            ConfirmationFlageLabel.ForeColor = System.Drawing.Color.Green;
            ConfirmationFlageLabel.Text = "Rules update Succesfull.";
            FormReset();

        }
        else
        {
            ConfirmationFlageLabel.ForeColor = System.Drawing.Color.Red;
            ConfirmationFlageLabel.Text = "Failed to Update Rules!";
            //FormRest();
        }
    }

    public bool CancelRules()
    {
        ProcessRulesData _unitRules = new ProcessRulesData();
        RulesData _units = new RulesData();
        bool cStatus = false;

        //foreach (DataListItem item in CancelUnitDataList.Items)
        //{
            //CheckBox chkUnit = (CheckBox)item.FindControl("UnitCheckBox");
            if (_ddlUnits.SelectedIndex > 0)
            {
                
                //Label UnitID = (Label)item.FindControl("lblUnitID");
                _units.UnitID = int.Parse(_ddlUnits.SelectedValue.ToString());
                _unitRules.RulesData = _units;
                _unitRules.CancelRules();
                cStatus = true;
            }

        
        
       
        //if (cStatus)
        //{
        //    ConfirmationFlageLabel.ForeColor = System.Drawing.Color.Green;
        //    ConfirmationFlageLabel.Text = "Rules update Succesfull.";
        //    cStatus = true;
        //    //FormRest();

        //}
        //else
        //{
        //    ConfirmationFlageLabel.ForeColor = System.Drawing.Color.Red;
        //    ConfirmationFlageLabel.Text = "Failed to Update Rules!";
        //    cStatus = false;
        //    //FormRest();
        //}

        return cStatus;




    }
    protected void RulesSubjectDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (RulesSubjectDropDownList.SelectedIndex > 0)
        //{
        //    LoadUnits();
        //}
    }
    protected void EditButton_Click(object sender, EventArgs e)
    {
        //bool cStatus = CancelRules();
        EditRules();


    }

    private void FormReset()
    {
        EmailTextBox.Text = "";
        SubjectTextBox.Text = "";
        DescriptionTextBox.Text = "";
    }
    
    protected void _ddlUnits_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUnitRulesInfo();
    }


}
