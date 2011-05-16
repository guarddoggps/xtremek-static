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

public partial class TermsNCondition : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    
    

    //protected void _btnAccept_Click1(object sender, EventArgs e)
    //{
    //    Response.Redirect("NewAccount.aspx");
    //}
    protected void _btnAccept_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewAccount.aspx");
    }
    protected void _btnDecline_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateNewAccount.aspx");
    }
}
