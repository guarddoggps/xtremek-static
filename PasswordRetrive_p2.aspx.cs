

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
using System.Data.SqlClient;
using AlarmasABC.BLL.ProcessUser;
using AlarmasABC.DAL.Queries;
using AlarmasABC.Utilities;

public partial class CompanyAdmin_PasswordRetrive_SQ : System.Web.UI.Page
{
	
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Session["securityQID"] == null)
		{
         	Response.Redirect("PasswordRetrive.aspx");
		}

        if (!IsPostBack)
        {
            loadSecurityQuestion();
        }
    }

    private void loadSecurityQuestion()
    {
        try
        {
			DataSet _ds = new DataSet();
			ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet("SELECT question FROM tblSecurityQuestion WHERE" + 
								  " id = " + Session["securityQID"].ToString());
			if (_ds.Tables[0].Rows.Count > 0) 
			{
				lblQuestion.Text = _ds.Tables[0].Rows[0]["question"].ToString();
			}
        }
        catch (Exception ex)
        {
           Console.WriteLine("PasswordRetrive::loadSecurityQuestion(): " + ex.Message.ToString());
        }
    }


    private void sendMessage()
    {
        DataSet _ds = new DataSet();

        try
        {
			ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet("SELECT password FROM tblUser WHERE uID = " + Session["userID"].ToString());
			if (_ds.Tables[0].Rows.Count > 0)
			{
		        string password = EncDec.GetDecryptedText(_ds.Tables[0].Rows[0]["password"].ToString());

		       	string subject = "Password Recovery";
				string body = "This message was sent to you because you";
				body += " reqested to recover a lost password. \r\n\r\n";
		        body += "Login Name: " + Session["userName"].ToString() + " \r\n";
		        body += "Password: " + password + "\r\n\r\n";

				SendEmail(Session["email"].ToString(), subject, body);

                lblMessage.Text = "Your password has been sent to your email address.";
                lblMessage.ForeColor = System.Drawing.Color.Green;
				txtAnswer.Text = "";
				
				Session["userID"] = null;
				Session["userName"] = null;
				Session["securityQID"] = null;
				Session["email"] = null;
			}
            else
            {
               lblMessage.Text = "There was an error trying to recover your password.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
            
        }
        catch (Exception ex)
        {           
			Console.WriteLine("PasswordRetrive::sendMessage(): " + ex.Message.ToString());
        }
        finally
        {
			_ds = null;
        }
    }

	private bool isCorrect(string answer)
	{
        try
        {
			DataSet _ds = new DataSet();
			ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet("SELECT securityAnswer FROM tblUser WHERE" + 
								  " uID = " + Session["userID"].ToString());
			if (_ds.Tables[0].Rows.Count > 0) 
			{
				if (answer == _ds.Tables[0].Rows[0]["securityAnswer"].ToString())
				{
					return true;
				}
			}
        }
        catch (Exception ex)
        {           
			Console.WriteLine("PasswordRetrive::isCorrect(): " + ex.Message.ToString());
        }

		return false;
	}
   
	private void SendEmail(string email, string subject, string text)
    {
        try
        {
            // Send contact email....
            Mailer.SendMailMessage("webmaster@xtremek.com", email, "", "",
                                       subject, text);
        }
        catch (Exception ex)
        {
            Console.WriteLine("SendMail(): " + ex.Message.ToString());
        }
    }

    private bool isValidData()
    {
        if (txtAnswer.Text == "")
        {
            lblMessage.Text = "Please Enter Security Answer.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        return true;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (isValidData())
        {
			if (isCorrect(txtAnswer.Text))
			{
				sendMessage();
			}
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
		Response.Redirect("PasswordRetrive.aspx");
    }
}
