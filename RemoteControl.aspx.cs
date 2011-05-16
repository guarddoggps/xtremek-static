using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
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
using AlarmasABC.BLL.ProcessUnitManagement;
using AlarmasABC.BLL.ProcessUnitType;
using AlarmasABC.Core.Admin;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL;
using AlarmasABC.Utilities.Security;

public partial class XtremeK_RemoteControl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        status_div.Visible = false;
        password_div.Visible = false;
        if (!IsPostBack)
        {   
            Session["msgShown"] = false;
            loadUnit();  
            displayLastAlert();
        }
    }

    private void loadUnit()
    {
        if (Request.QueryString["unitID"] != null)
        {
            try
            {
                string strSQL = "SELECT unitName,deviceID FROM tblUnits WHERE unitID = '" + Request.QueryString["unitID"] + "' AND comID = " + Session["trkCompany"].ToString() + ";";
                ExecuteSQL exec = new ExecuteSQL();
                DataSet ds = new DataSet();

                ds = exec.getDataSet(strSQL);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["deviceID"] = ds.Tables[0].Rows[0]["deviceID"].ToString();
                    unitName.Text = "Selected Unit: " + ds.Tables[0].Rows[0]["unitName"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("loadUnit(): " + ex.Message.ToString());
                Console.WriteLine("loadUnit(): " + ex.Message.ToString());
            }
        }
    }

    private string getUnitName()
    {
        try
        {
            string strSQL = "SELECT unitName FROM tblUnits WHERE deviceID = " + Session["deviceID"] + " AND comID = " + Session["trkCompany"].ToString() + ";";
            ExecuteSQL exec = new ExecuteSQL();
            DataSet ds = new DataSet();

            ds = exec.getDataSet(strSQL);

            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["unitName"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("getUnitName(): " + ex.Message.ToString());
            Console.WriteLine("getUnitName(): " + ex.Message.ToString());
        }

        return "";
    }

    private void sendUnitCommand(string command)
    {
        UnitCommand unitCommand = new UnitCommand();

        try
        {
            unitCommand.SendUnitCommand(int.Parse(Session["deviceID"].ToString()), command);
          
            lblMessage.Text = "Sent command successfully!";
            lblMessage.ForeColor = System.Drawing.Color.Green;
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine("UnitCommands:sendUnitCommand(): " + ex.Message.ToString());
        }
        finally
        {
            unitCommand = null;
        }

        lblMessage.Text = "Error while sending command!";
        lblMessage.ForeColor = System.Drawing.Color.Red;
    }

    private bool checkPassword()
    {
        string strSQL = "SELECT password FROM tblUser WHERE uID = " + Session["uID"].ToString();
        ExecuteSQL exec = new ExecuteSQL();
        DataSet ds = new DataSet();

        ds = exec.getDataSet(strSQL);

        if (ds.Tables[0].Rows.Count > 0) 
        {
            if (ds.Tables[0].Rows[0]["password"].ToString() == EncDec.GetEncryptedText(txtPassword.Text))
            {      
                return true;
            }
        }

        return false;
    }

    private void runCommand(string cmd)
    {
        password_div.Visible = true;
        Timer.Enabled = false;
        Session["command"] = cmd;
    }

    public AlertData getLastAlert()
    {
        string strSQL = "SELECT * FROM tblAlert WHERE unitID = " +
                        "(SELECT DISTINCT unitID FROM tblUnits WHERE deviceID = " + Session["deviceID"].ToString() + 
                        " AND comID = " + Session["trkCompany"].ToString() + ") ORDER BY id DESC LIMIT 1;";
        ExecuteSQL exec = new ExecuteSQL();
        DataSet ds = new DataSet();
        AlertData alert = new AlertData();


        try
        {
            ds = exec.getDataSet(strSQL);

            if (ds.Tables[0].Rows.Count > 0)
            {
                alert.AlertTime = ds.Tables[0].Rows[0]["alertTime"].ToString();
                alert.AlertMessage = ds.Tables[0].Rows[0]["alertMessage"].ToString();
                alert.AlertType = ds.Tables[0].Rows[0]["alertType"].ToString();

            }
        }
        catch (Exception ex)
        {
            throw new Exception ("getLastAlert(): " + ex.Message + "\n" + strSQL);
        }

        return alert;
    }

    private void displayLastAlert()
    {
        AlertData alert;

        alert = getLastAlert();

        lblMessage.Text = alert.AlertTime + ": " + alert.AlertMessage;
        if (alert.AlertType == "Event")
        {
            lblMessage.ForeColor = Color.FromArgb(5, 255, 242, 106);
        }
        else if (alert.AlertType == "Red Alert" || alert.AlertType == "Speeding" || 
                 alert.AlertType == "Geofence")
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void Timer_Tick(object sender, EventArgs e)
    {
        displayLastAlert();
    }

    public void btnLock_Click(object sender, EventArgs e)
    {
        runCommand("20,70");
    }
    public void btnUnlock_Click(object sender, EventArgs e)
    {
        runCommand("20,20");
    }

    public void btnSilentAlarm_Click(object sender, EventArgs e)
    {
        runCommand("20,69");
    }
    public void btnPanicAlarm_Click(object sender, EventArgs e)
    {
        runCommand("20,31");
    }

    public void btnStopEngine_Click(object sender, EventArgs e)
    {
        runCommand("20,71");
    }
    public void btnStartEngine_Click(object sender, EventArgs e)
    {
        runCommand("20,72");
    }
    
    public void btnAuxilary_Click(object sender, EventArgs e)
    {
        runCommand("20,98");
    }
    
    public void btnClose_Click(object sender, EventArgs e)
    {
        Timer.Enabled = true;
        if (status_div.Visible == true) 
        {
            status_div.Visible = false;
        }
        
        if (password_div.Visible == true)
        {
            password_div.Visible = false;
            Session["command"] = null;
        }
    }

    public void btnSubmit_Click(object sender, EventArgs e)
    {
        if (checkPassword())
        {
            sendUnitCommand(Session["command"].ToString());
            Session["command"] = null;
            Timer.Enabled = true;
        }
    }

    public void btnStatus_Click(object sender, EventArgs e)
    {
        status_div.Visible = true;
        Timer.Enabled = false;

        try
        {
            AlertData alert = getLastAlert();

            string deviceID = Session["deviceID"].ToString();
            string strSQL = "SELECT lat,long,recTimeRevised AS recDateTime,velocity,deviceID,unitName FROM unitRecords WHERE recTimeRevised = '" + 
                            alert.AlertTime + "' AND deviceID = " + deviceID;
            ExecuteSQL exec = new ExecuteSQL();
            DataSet ds = new DataSet();
            ds = exec.getDataSet(strSQL);

            unitInfo.Text =  "Type: " + alert.AlertType + "<br/>";
            unitInfo.Text += "Message: " + alert.AlertMessage + "<br/><br/>";

            unitInfo.Text += MapDataCommon.GetCoreDataSetMarkerText(ds, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine("btnStatus_Click(): " + ex.Message);
        }
    }

}
