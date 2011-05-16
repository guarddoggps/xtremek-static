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



public partial class Tracking_Speeding : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            _chkOn.Checked = true;
           // haveData.Value = System.Convert.ToString(ShowRulesDetails());
           // string test = "";
            ShowRulesDetails();

        }

    }

   private void ShowRulesDetails()
    {
        try
        {
            ProcessSpeedingQueries _rulesData = new ProcessSpeedingQueries();
            RulesData _rulesInfo = new RulesData();
            _rulesInfo.ComID = int.Parse(Session["trkCompany"].ToString());
            _rulesInfo.UnitID = int.Parse(Request.QueryString["unitID"].ToString());

            _rulesData.RulesInfo = _rulesInfo;

            _rulesData.invoke();
            DataSet _ds = new DataSet();
            _ds = _rulesData.Ds;

            if (_ds.Tables[0].Rows.Count > 0)
            {
                _txtSpeedValue.Text = _ds.Tables[0].Rows[0]["rulesValue"].ToString();
                _txtNotificationEmails.Text = _ds.Tables[0].Rows[0]["email"].ToString();
                _txtMessage.Text = _ds.Tables[0].Rows[0]["description"].ToString();
                _txtPhoneNumber.Text = _ds.Tables[0].Rows[0]["speedPhoneNum"].ToString();
                bool isActive = bool.Parse(_ds.Tables[0].Rows[0]["isActive"].ToString());
                ViewState["rulesID"] = _ds.Tables[0].Rows[0]["rulesID"].ToString();

                if (isActive)
                {
                    _chkOff.Checked = false;

                }
                else
                {
                    _chkOff.Checked = true;
                    _chkOn.Checked = false;

                }
                bool isSMS = false;
                string tmp = _ds.Tables[0].Rows[0]["isSMS"].ToString();
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
                _btnOk.Text = "Assign Rules";
            }
        }
        catch (Exception ex)
        { 
        
        }
        
    }
  
    public void AssignRules()
    {
        RulesData _rules = new RulesData();

        ProcessSpeedingNotQueries insert = new ProcessSpeedingNotQueries(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

        try
        {
            _rules.RulesName = "";
            _rules.RulesValue = _txtSpeedValue.Text.ToString();
            _rules.Email = _txtNotificationEmails.Text.ToString();

            bool isSMS = _ckOnSMS.Checked;
            if (_txtPhoneNumber.Text == "")
            {
		isSMS = false;
            }
            _rules.SpeedingPhoneNum = _txtPhoneNumber.Text.Trim();
            _rules.IsSMS = isSMS;
            _rules.Message = _txtMessage.Text.ToString();
            string _comID = Session["trkCompany"].ToString();
            _rules.ComID = int.Parse(_comID);
            _rules.UnitID =int.Parse( Request.QueryString["unitID"].ToString());

            insert.RulesInfo = _rules;
            insert.invoke();

          
            _lblMessage.ForeColor = System.Drawing.Color.Green;
           _lblMessage.Text = "Rules Assign Successfully.";


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
           _lblMessage.ForeColor = System.Drawing.Color.Red;
           _lblMessage.Text = "Rules Assign fail.";
        }
        finally
        {
            _rules = null;
        }
    }
    protected void OkButton_Click(object sender, EventArgs e)
    {
        if (_btnOk.Text == "Assign Rules")
        {
            AssignRules();
        }
        else if (_btnOk.Text == "Update")
        {
            UpdateRules();
        }
    }

    public void UpdateRules()
    {
        RulesData _rules = new RulesData();

        ProcessSpeedingNotQueries update = new ProcessSpeedingNotQueries(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);

        try
        {
            bool _status = false;
	    bool isSMS = _ckOnSMS.Checked;

            _status = _chkOn.Checked;
           
            _rules.RulesValue = _txtSpeedValue.Text.ToString();
            _rules.Email = _txtNotificationEmails.Text.ToString();
            _rules.Message = _txtMessage.Text.ToString();
            _rules.SpeedingPhoneNum = _txtPhoneNumber.Text.Trim();
            if (_txtPhoneNumber.Text == "")
            {
		isSMS = false;
            }
            _rules.IsSMS = isSMS;
            _rules.UnitID = int.Parse( Request.QueryString["unitID"].ToString());
            _rules.RulesID = int.Parse(ViewState["rulesID"].ToString());
            _rules.IsActive = _status;

            update.RulesInfo = _rules;
            update.invoke();

           
            _lblMessage.ForeColor = System.Drawing.Color.Green;
            _lblMessage.Text = "Rules Update Successfully.";


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Rules Update fail.";
        }
        finally
        {
            _rules = null;
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
