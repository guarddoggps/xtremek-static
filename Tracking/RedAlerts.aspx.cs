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
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessRulesData;
using AlarmasABC.DAL.Queries;


public partial class Tracking_RedAlerts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // haveData.Value = System.Convert.ToString(ShowRulesDetails());
           // string test = "";
            ShowRulesDetails();

        }

    }

   private void ShowRulesDetails()
    {
        try
        {
            int comID, unitID;
            string strSQL;
            ExecuteSQL exec = new ExecuteSQL();
  
            comID = int.Parse(Session["trkCompany"].ToString());
			unitID = int.Parse(Request.QueryString["unitID"].ToString());
            strSQL = "select * from tblAlertRules where comID = " + comID + " and unitID = " + unitID;
            
            DataSet ds = new DataSet();
            ds = exec.getDataSet(strSQL);

            if (ds.Tables[0].Rows.Count > 0)
            {
                _txtNotificationEmails.Text = ds.Tables[0].Rows[0]["email"].ToString();
                _txtPhoneNumber.Text = ds.Tables[0].Rows[0]["phoneNumber"].ToString();;
                bool isActive = bool.Parse(ds.Tables[0].Rows[0]["isActive"].ToString());

                if (isActive)
                {
                    _chkOn.Checked = true;
                    _chkOff.Checked = false;

                }
                else
                {
                    _chkOff.Checked = true;
                    _chkOn.Checked = false;

                }
                bool isSMS = false;
                string tmp = ds.Tables[0].Rows[0]["isSMS"].ToString();
                if (tmp.Length > 0)
                {
                    isSMS = bool.Parse(tmp);
                }

                if (isSMS)
                {
                    _ckOnSMS.Checked = true;
                    _chkOffSMS.Checked = false;
                }
                else
                {
                    _ckOnSMS.Checked = false;
                    _chkOffSMS.Checked = true;
                }

                _btnOk.Text = "Update";

            }
            else
            {
                _btnOk.Text = "Save";
            }
        }
        catch (Exception ex)
        { 
        
        }
        
    }
  
    public void SaveUpdateInfo(bool saveInfo)
    {
        try
        {
            int comID, unitID;
            bool isActive, isSMS;
            string strSQL;
            ExecuteSQL exec = new ExecuteSQL();
  
            isActive = _chkOn.Checked ? true : false;
            isSMS = _ckOnSMS.Checked ? true : false;

            comID = int.Parse(Session["trkCompany"].ToString());
            unitID = int.Parse(Request.QueryString["unitID"].ToString());
			if (saveInfo) 
			{
            	strSQL = "insert into tblAlertRules(unitID, comID, email, phoneNumber, isSMS, isActive)" +
                     	" VALUES (" + unitID + "," + comID + ",'" + _txtNotificationEmails.Text.ToString() + "','" +
                     	_txtPhoneNumber.Text.Trim() + "','" + isSMS + "','" + isActive + "');";
			} 
			else 
			{
            	strSQL = "update tblAlertRules set email = '" + _txtNotificationEmails.Text.ToString() + 
                      	"', phoneNumber = '" + _txtPhoneNumber.Text.Trim() + "', isSMS = '" + isSMS + "', isActive = '" + 
                      	isActive + "' where unitID = " + unitID + " and comID = " + comID + ";";
			}
       
           	exec.ExecuteNonQuery(strSQL);

            _lblMessage.ForeColor = System.Drawing.Color.Green;
			if (saveInfo) 
			{
	           _lblMessage.Text = "Saved info successfully.";
			}
			else
			{
            	_lblMessage.Text = "Updated info successfully.";
			}
        }
        catch (Exception ex)
        {
            Console.WriteLine("SaveUpdateInfo(): " + ex.Message.ToString());
           _lblMessage.ForeColor = System.Drawing.Color.Red;
           _lblMessage.Text = "Could not save info.";
        }
    }
    protected void OkButton_Click(object sender, EventArgs e)
    {
        if (_btnOk.Text == "Save")
        {
            SaveUpdateInfo(true);
        }
        else if (_btnOk.Text == "Update")
        {
            SaveUpdateInfo(false);
        }
    }

    protected void _chkOn_CheckedChanged(object sender, EventArgs e)
    {
        if (_chkOn.Checked)
        {
            _chkOff.Checked = false;
        }
        else if (!_chkOn.Checked)
        {
            _chkOff.Checked = true;
        }
    }
    protected void _chkOff_CheckedChanged(object sender, EventArgs e)
    {
        if (_chkOff.Checked)
        {
            _chkOn.Checked = false;
        }
        else if (!_chkOff.Checked)
        {
            _chkOn.Checked = true;
        }
    }
    protected void _ckOnSMS_CheckedChanged(object sender, EventArgs e)
    {
        _chkOffSMS.Checked = false;
    }
    protected void _chkOffSMS_CheckedChanged(object sender, EventArgs e)
    {
        _ckOnSMS.Checked = false;
    }
}
