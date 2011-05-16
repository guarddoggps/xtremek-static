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

public partial class CompanyAdmin_PasswordRetrive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			if (Session["userName"] != null && Session["email"] != null)
			{
                txtUserName.Text = Session["userName"].ToString();
				txtEmail.Text = Session["email"].ToString();

				Session["userName"] = null;
				Session["email"] = null;
			}
        }
    }

    private bool userIsValid()
    {
        try
        {
			string strSQL = "SELECT * FROM tblUser WHERE login = '" + txtUserName.Text.Trim() + 
								  "' AND email = '" + txtEmail.Text.Trim() + "';";
			DataSet _ds = new DataSet();
            ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet(strSQL);
			
			if (_ds.Tables[0].Rows.Count > 0)
			{
				Session["userID"] = _ds.Tables[0].Rows[0]["uID"].ToString();
				Session["userName"] = _ds.Tables[0].Rows[0]["login"].ToString();
				Session["securityQID"] = _ds.Tables[0].Rows[0]["securityQuestion"].ToString();
				Session["email"] = _ds.Tables[0].Rows[0]["email"].ToString();
			}
			else
			{
                lblMessage.ForeColor = System.Drawing.Color.Red;
				lblMessage.Text = "The supplied user name and email does not exist in our database.";
				return false;
			}
			
        }
        catch (Exception ex)
        {
            Console.WriteLine("PasswordRetrive::userIsValid(): " + ex.Message.ToString());
        }
        finally
        {
        }

		return true;
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
        if (txtUserName.Text == "")
        {
            lblMessage.Text = "Please Enter Login Name.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (txtEmail.Text == "")
        {
            lblMessage.Text = "Please Enter Email Address.";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        return true;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (isValidData())
        {
            if (userIsValid()) 
			{
				Response.Redirect("PasswordRetrive_p2.aspx");
			}
        }
    }
}
