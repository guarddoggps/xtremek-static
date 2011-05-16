using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using AlarmasABC.BLL.ProcessPermission;
using AlarmasABC.Core.Admin;



/// <summary>
/// Summary description for FormPermission
/// </summary>
public static class FormPermission
{
  
    public static DataSet _ds = new DataSet();

  
    public static DataSet LoadPermission()
    {
        

       
        try
        {
            ProcessSchemePermission psp = new ProcessSchemePermission();
            User userObj = new User();

            userObj.UID = int.Parse (HttpContext.Current.Session["uID"].ToString());

            psp.UObj = userObj;
            psp.GetSchemePermission();

            _ds = psp.Ds;

            
              

                    
        }
        catch (Exception ex)
        {
			Console.WriteLine("FormPermission::LoadPermission(): " + ex.Message.ToString());
            //return formIDs;
        }

        finally
        {
            

        }

        return _ds;

    }
}
