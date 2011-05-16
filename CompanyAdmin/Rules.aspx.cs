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

public partial class CompanyAdmin_Rules : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["trkCompany"] = 1;
         
        
    }
    protected void RulesMenu_MenuItemClick(object sender, MenuEventArgs e)
    {
        _mltView.ActiveViewIndex = Int32.Parse(e.Item.Value);
        int i;
        int index = _mltView.ActiveViewIndex;
        //if(Session["insertSuccess"]!=null)
        //RulesMultiView.ActiveViewIndex = (int)Session["insertSuccess"];
        //Make the selected menu item reflect the correct imageurl

        switch (index)
        {
            case 0:
                RulesMenu.Items[0].ImageUrl = "~/Images/RSetup_over.gif";
                deselectedMenu(0);
                break;
            case 1:
                RulesMenu.Items[1].ImageUrl = "~/Images/AssignRules_over.gif";
                deselectedMenu(1);
                break;

            case 2:
                RulesMenu.Items[2].ImageUrl = "~/Images/EditRules_over.gif";
                deselectedMenu(2);
                break;

            default:
                RulesMenu.Items[0].ImageUrl = "~/Images/RSetup_over.gif";
                deselectedMenu(0);
                break;
        }
    }

    private void deselectedMenu(int index)
    {
        for (int i = 0; i < RulesMenu.Items.Count; i++)
        {
            if (i != index)
                switch (i)
                {
                    case 0:
                        RulesMenu.Items[0].ImageUrl = "~/Images/RSetup_normal.gif";
                        break;
                    case 1:
                        RulesMenu.Items[1].ImageUrl = "~/Images/AssignRules_normal.gif";
                        break;

                    case 2:
                        RulesMenu.Items[2].ImageUrl = "~/Images/EditRules_normal.gif";
                        break;

                }
        }
    }
}
