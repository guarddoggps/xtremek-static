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
using System.Text;
using System.Xml.Linq;
using AlarmasABC.Core.Admin;
using AlarmasABC.DAL.Select;
using AlarmasABC.BLL.ProcessLogin;
using AlarmasABC.Utilities.Security;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		// Called upon a postback (such as a button click)
        if (IsPostBack)
        {
			// Get the timezone value from the hidden field on the Login page.
			// When the page was initially rendered, the Javascript filled the
			// hidden field with the timezone, and now we are reading the value
			// upon postback and set the timezone session variable, so this is 
			// read only once.
			XtremeK.TimeZone.timezone = int.Parse(_timezone.Value);
		}
		else
		{
        	txtLogin.Focus();
		}
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        loginUser();
    }

    private void loginUser()
    {
        bool success = false;
        DataSet ds = new DataSet();
        AlarmasABC.Core.Admin.Login login = new AlarmasABC.Core.Admin.Login();
        ProcessLogin processLogin = new ProcessLogin();

        try
        {
            login.ComName = "XtremeK";
            login.LoginName = txtLogin.Text;
            login.PassWord = EncDec.GetEncryptedText(txtPassword.Text);

            processLogin.UserLogin = login;
            processLogin.invoke();
            ds = processLogin.Ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["trkCompany"] = ds.Tables[0].Rows[0]["comID"].ToString();
                Session["uID"] = ds.Tables[0].Rows[0]["uID"].ToString();
                Session["UserName"] = ds.Tables[0].Rows[0]["userName"].ToString();
                Session["RoleID"] = ds.Tables[0].Rows[0]["roleID"].ToString();
                Session["CompanyName"] = login.ComName;
                success = true;
            }
            else
            {
                lblMessage.Text = "Invalid user name or password.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(" Login " + ex.Message);
        }

        finally
        {
            login = null;
            processLogin = null;
            ds = null;
        }

        // If the username and password are correct, redirect to the home page
        if (success)
        {
            Response.Redirect("Home.aspx");
        }
    }
}
