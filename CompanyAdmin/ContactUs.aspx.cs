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
using System.IO;
using AlarmasABC.Core.Admin;
using AlarmasABC.BLL.ProcessContactInfo;
using AlarmasABC.DAL.Queries;
using AlarmasABC.Utilities;

public partial class CompanyAdmin_ContactUs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            _txtUserName.Text = Session["UserName"].ToString();
        }
        catch (Exception ex)
        { 
        
        }
        
    }
        
    protected void _btnSubmit_Click(object sender, EventArgs e)
    {
        if (validData())
            SaveContactInfo();
    }

    private bool validData()
    {
        if (_txtEmail.Text == "")
        {
            _lblMessage.Text = "Please enter email ID";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (_ddlDepartment.SelectedIndex < 1)
        {
            _lblMessage.Text = "Please select a department.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (_tbDesc.Text == "")
        {
            _lblMessage.Text = "Please Enter Contact Description.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        return true;
    }

    private void SaveContactInfo()
    {
        bool noError = false;

        try
        {
            // Save contact to database
            Contact _conInfo = new Contact();
            ProcessContact _pCon = new ProcessContact(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

            _conInfo.UserName = Session["UserName"].ToString();
            _conInfo.Email = _txtEmail.Text.ToString();
            _conInfo.Department = _ddlDepartment.SelectedValue.ToString();
            _conInfo.Description = _tbDesc.Text.ToString();

            _pCon.ConInfo = _conInfo;
            _pCon.invoke();        

            // Send mail message to the appropriate department
            SendEmail(_conInfo.Email, _conInfo.Department, _conInfo.Description);

            _lblMessage.Text = "Message send successful";
            formReset();
            noError = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            _lblMessage.Text = "Message Send fail " + ex.Message.ToString();
        }
        finally
        {
            if(noError)
            {
                Response.Redirect("thankYouPage.aspx");
            }
        }
    }

    private void SendEmail(string email, string department, string description)
    {
        string subject = "Support request from " + Session["UserName"].ToString() +
                         " (" + department + ")";
        try
        {
            // Send contact email....
            DataSet ds = new DataSet();
            string strSQL = "SELECT email,adminEmail FROM tblContact WHERE department = '" + 
                            department + "';";
            ExecuteSQL exec = new ExecuteSQL();
            ds = exec.getDataSet(strSQL);

            if (ds.Tables[0].Rows.Count < 1) 
            {
                throw new Exception("Couldn't get any data from query!");
            }

            if (ds.Tables[0].Rows[0]["email"].ToString().Length > 0)
            {
                Mailer.SendMailMessage(email, ds.Tables[0].Rows[0]["email"].ToString(), 
                                       ds.Tables[0].Rows[0]["adminEmail"].ToString(), "",
                                       subject, description);
            }     

            Mailer.SendMailMessage(email, ds.Tables[0].Rows[0]["adminEmail"].ToString(), "", "",
                                   subject, description);

        }
        catch (Exception ex)
        {
            Console.WriteLine("SendMail(): " + ex.Message.ToString());
        }
    }

    private void formReset()
    {
        _txtEmail.Text = "";
        _tbDesc.Text = "";
        _ddlDepartment.SelectedIndex = 0;
    }
    protected void _btnReset_Click(object sender, EventArgs e)
    {
        formReset();
    }
}
