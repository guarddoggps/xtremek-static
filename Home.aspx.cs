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
using Telerik.Web.UI;
using System.Text;
using System.Timers;
using AlarmasABC.Core.Admin;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessUnit;
using AlarmasABC.BLL.ProcessTreeData;
using AlarmasABC.BLL.TreeColor;
using AlarmasABC.BLL.ProcessCompany;
using AlarmasABC.DAL;
using AlarmasABC.DAL.Queries;
using AlarmasABC.Utilities.Security;

public partial class Home : System.Web.UI.Page,ICallbackEventHandler
{

    #region Global Variables

    string _str;
    DataSet _dsTree;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check the session user ID. If it's null, go back to the login page....
        if (Session["uID"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (!IsPostBack)
        {
            ClientScriptManager m = Page.ClientScript;
            string str = m.GetCallbackEventReference(this, "args", "ReceiveServerData", "'this is context from server'");
            string strCallback = "function CallServer(args,context){" + str + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", strCallback, true);


            if (Session["UserName"] != null)
                _lblUserName.Text = Session["UserName"].ToString();         
            

			if (Session["Unit"] != null) Session["Unit"] = null;
            LoadUnits();

            string roleID = Session["RoleID"].ToString();
            if (roleID != "1" && roleID != "2")
            {
                setUserRights();
            }
        }

    }

    public string GetCallbackResult()
    {
        return _str;

    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        _str = setHelpText(eventArgument);
    }


    private void RegisterJavaScript()
    {
        StringBuilder _script = new StringBuilder();

        _script.Append("<script language=\"javascript\"");
        _script.Append("function SetHistoricalMap()");
        _script.Append("{");
        _script.Append("try");
        _script.Append("{");
        _script.Append("var splitter = $find('<%= RadSplitter1.ClientID %>');");
        _script.Append("var pane = $find('<%= MapPane.ClientID %>');");
        _script.Append("var iframe = pane.getExtContentElement(); ");
        _script.Append("var href=iframe.contentWindow.document.location;");
        _script.Append("var url=href.toString(); ");
        _script.Append("if(url.search('GDMap')!=-1){");
        _script.Append("url=url.replace('GDMap','HGMap'); pane.set_contentUrl(url); }");
        _script.Append(" else if(url.search('BreadCrumbs')!=-1){");
        _script.Append("url=url.replace('BreadCrumbs','HGMap'); pane.set_contentUrl(url); }  ");
        _script.Append("else if(url.search('HGMap')!=-1) {");
        _script.Append("}}");
        _script.Append("catch(E){alert(E.dexcription);}");
        _script.Append("}");
        _script.Append("</script>");

        ScriptManager.RegisterClientScriptBlock(this, Page.GetType(), "Script", _script.ToString(), false);
    }

    private string getTreeColor(int unitID)
    {
        int comID = int.Parse(Session["trkCompany"].ToString());
        string strSQL;
        DataSet ds = new DataSet();
        ExecuteSQL exec = new ExecuteSQL();

        strSQL = "select * from tblAlert where unitID = " + unitID.ToString() + " and comID = " 
                 + comID.ToString() + " and alertTime > 'now'::timestamp" +
                 " - interval '5 minutes' order by alertTime limit 1";

        try 
        {
            ds = exec.getDataSet(strSQL);
        }
        catch (Exception ex)
        {
            Console.WriteLine("getTreeColor(): " + ex.Message.ToString());
        }

        if (ds.Tables[0].Rows.Count < 1) 
        {
			strSQL = "select deviceID from tblUnits where unitID = " + unitID.ToString() + 
					 " and comID = " + comID.ToString();
			ds = exec.getDataSet(strSQL);
			int deviceID ;

			if (ds.Tables[0].Rows.Count > 0) {
				deviceID = int.Parse(ds.Tables[0].Rows[0]["deviceID"].ToString());
			} else {
				return "Green";
			}

            DataSet ds2 = new DataSet();
            strSQL = "select * from tblGPRS where deviceID = " + deviceID +
				     " and recTimeRevised > 'now'::timestamp - interval '12 hours' limit 1";
            ds2 = exec.getDataSet(strSQL);

            if(ds2.Tables[0].Rows.Count < 1) {
				strSQL = "select * from tblGPRS where deviceID = " + deviceID + " limit 5";
				ds2 = exec.getDataSet(strSQL);
				
				if (ds2.Tables[0].Rows.Count < 1) {
					return "Black";
				} else {
                	return "Gray";	
				}
            }
            return "Green";
        } 
        else
        {
            string alertType = ds.Tables[0].Rows[0]["alertType"].ToString();
            if (alertType == "Event") 
            {
                return "Yellow";
            }
            else
            {
                return "Red";
            }
        }
    }

	private string getItemText(string text, string deviceID, string unitID)
	{
		
		if (text == "History")
        {
         	return "<a class=\"subItem\" onclick= \" ReLoadWindow('Tracking/Historical.aspx?deviceID=" + deviceID + "','Unit History',530,455)\"> " + text + "</a>";	
        }
		else if (text == "Mini Map")
        {
            return "<a onclick= \" ReLoadWindow('Map/MiniMap.aspx?deviceID=" + deviceID + "','Minimap',380, 580)\" > " + text + "</a>";
        } 
		else if (text == "Alerts")
        {
           	return "<a onclick=\"ReLoadWindow('Tracking/Alerts.aspx?unitID=" + unitID + "','Alerts',540, 340)\" > " + text + "</a>";
        }
		else if (text == "Red Alerts")
		{
			return "<a onclick=\"ReLoadWindow('Tracking/RedAlerts.aspx?unitID=" + unitID + "','Red Alerts Setup',440, 330)\" >Red Alerts</a>";
		}
		else if (text == "Speeding")
		{
			return "<a onclick=\"ReLoadWindow('Tracking/Speeding.aspx?unitID=" + unitID + "','Speeding Alerts Setup',450, 410)\" >Speeding</a>";
		}
		else if (text == "Safety Zones")
		{
			return "<a onclick=\"ReLoadWindow('Tracking/SafetyZones.aspx?unitID=" + unitID + "','Safety Zones Setup',550, 500)\" >Safety Zones</a>";
		}

		return null;
	}

	private void LoadUnits()
	{
		try
		{
			if (menuUnits.Items.Count > 0)
				menuUnits.Items.Clear();

			int _comID = int.Parse(Session["trkCompany"].ToString());
            int _uID = int.Parse(Session["uID"].ToString());

            string _treeCSS = "";
            string ComID = Session["trkCompany"].ToString();
            string userID = Session["uID"].ToString();
            string[] _subMenu = { "History", "Mini Map", "Alerts", "Red Alerts", "Speeding", "Safety Zones" };


            //DateTime _dtInterval = Convert.ToDateTime("01/01/2000");
            Hashtable _hTreeColor = new Hashtable();
            DataTable _dt = new DataTable();

            ProcessTreeData _processTree = new ProcessTreeData();
            _processTree.ComID = _comID;
            _processTree.UID = _uID;
            _processTree.invoke();
            _dsTree = _processTree.Ds;

            _dt = _dsTree.Tables[2];
            _dsTree.Relations.Add("ParentChild", _dsTree.Tables[0].Columns["typeID"], _dsTree.Tables[1].Columns["typeID"]);

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                 int unitID = int.Parse(_dt.Rows[i][0].ToString());
                _hTreeColor.Add(unitID.ToString(), getTreeColor(unitID));
            }

            if (!isDemoUser()) 
            {
                RadMenuItem unitControlHeader = new RadMenuItem();
                unitControlHeader.Text = "Unit Control";
                unitControlHeader.CssClass = "menuHeader";
                menuUnits.Items.Add(unitControlHeader);

                RadMenuItem remoteControl = new RadMenuItem();
                remoteControl.Text = "Remote Control";
                menuUnits.Items.Add(remoteControl);
            }

			RadMenuItem unitTypeHeader = new RadMenuItem();
			unitTypeHeader.Text = "Unit Groups";
			unitTypeHeader.CssClass = "menuHeader";
			menuUnits.Items.Add(unitTypeHeader);

            foreach (DataRow _rowParent in _dsTree.Tables[0].Rows)
            {
                int i = 0;

				RadMenuItem unitTypeItem = new RadMenuItem();
				unitTypeItem.Text = " " + _rowParent["typeName"].ToString() + " (" + countChildNode(Convert.ToInt32(_rowParent["typeID"])) + ")";
			 	unitTypeItem.Target = MapPane.ClientID;
			 	//unitTypeItem.NavigateUrl = "Map/MainMap.aspx?tID=" + _rowParent["typeID"].ToString();

                foreach (DataRow _rowChild in _rowParent.GetChildRows("ParentChild"))
                {
                    i++;

					string _unitID = _rowChild["unitID"].ToString();

					RadMenuItem unitItem = new RadMenuItem();
					if (i <= 9) 
                    {
                        unitItem.Text = " 0" + i + ". " + _rowChild["unitName"].ToString();
                    }
                    else 
                    {
                        unitItem.Text = " " + i + ". " + _rowChild["unitName"].ToString();
                    }
			 		unitItem.Target = MapPane.ClientID;
			 		unitItem.NavigateUrl = "Map/BreadCrumbs.aspx?deviceID=" + _rowChild["deviceID"].ToString() + " ";
					unitItem.Attributes["deviceID"] = _rowChild["deviceID"].ToString();
					unitItem.Attributes["unitID"] = _unitID;
					unitTypeItem.Items.Add(unitItem);

					try
                    {
                        _treeCSS = _hTreeColor[_unitID].ToString();
                        unitItem.CssClass = _treeCSS;
                    }
                    catch (Exception ex)
                    {
						Console.WriteLine(ex.Message.ToString());
                    }
                }
				
				if (unitTypeItem.Items.Count > 10) {
					unitTypeItem.GroupSettings.Height = 280;
				}
  
				menuUnits.Items.Add(unitTypeItem);
		
            }
			if (menuUnits.Items.Count > 14) {
				menuUnits.GroupSettings.Height = 280;
			}
		}
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
	}

    
    private int countChildNode(int _typeID)
    {
        int _comID = int.Parse(Session["trkCompany"].ToString());
        int _uID = int.Parse(Session["uID"].ToString());
        int rc = 1;

        ProcessUnitCount _unitCount = new ProcessUnitCount();
        try
        {
            _unitCount.ComID = _comID;
            _unitCount.UID = _uID;
            _unitCount.TypeID = _typeID;
            _unitCount.invoke();
            DataSet _ds = new DataSet();
            _ds = _unitCount.Ds;

            rc = int.Parse(_ds.Tables[0].Rows[0]["unitCount"].ToString());
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _unitCount = null;
        }

        return rc;
    }      


    protected void Logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

    public string GetHelpText(string _panelName)
    {
        return _panelName;
    }

    public string setHelpText(string tabName)
    {
        try
        {
            DataSet ds = new DataSet();
            string path = Server.MapPath("XML");
            ds.ReadXml(path + "//FooterInfo.xml");

            if (tabName == "2")
                return ds.Tables[0].Rows[0]["Admin"].ToString();
            else if (tabName == "3")
                return ds.Tables[0].Rows[0]["Report"].ToString();
            else if (tabName == "1")
                return ds.Tables[0].Rows[0]["Unit"].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return "";

    }

	protected void _Timer_Tick(object sender, EventArgs e)
    {
        try
        {
            LoadUnits();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    private bool isDemoUser()
    {
        string _userID = Session["uID"].ToString();
        string _comID = Session["trkCompany"].ToString();
        string _strSQL = "SELECT login FROM tblUser WHERE uID = " + _userID + " AND comID = " + _comID;
        
        DataSet _ds = new DataSet();
        ExecuteSQL exec = new ExecuteSQL();
        _ds = exec.getDataSet(_strSQL);
        
        if (_ds.Tables[0].Rows.Count > 0)
        {
            Console.WriteLine(_ds.Tables[0].Rows[0]["login"]);
            if (_ds.Tables[0].Rows[0]["login"].ToString() == "demo") 
            {
                return true;
            }
        }
        
        return false;
    }
    
    private void setUserRights()
    {
        string _userID = Session["uID"].ToString();
        string _comID = Session["trkCompany"].ToString();
        bool _hasPermission = false;
        Telerik.Web.UI.RadMenuItem _menu = new RadMenuItem();

        if (isDemoUser())
        {
            // Remote all from admin
            foreach (RadMenuItem item in menuAdministration.Items)
            {
                item.Visible = false;
            }
            
            // Remove all from fleet
            foreach (RadMenuItem item in menuFleet.Items)
            {
                item.Visible = false;
            }
        }
        else 
        {
            string _strSQL = "SELECT id,moduleName FROM tblModule";


            DataSet _ds = new DataSet();
            ExecuteSQL exec = new ExecuteSQL();
            _ds = exec.getDataSet(_strSQL);

            for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
            {
                _strSQL = "SELECT * FROM tblForms WHERE id IN";
                _strSQL += " (SELECT formID FROM tblSchemePermission WHERE schemeID = ";
                _strSQL += " (SELECT MAX(schemeID) FROM tblUserWiseScheme WHERE userID = ";
                _strSQL += _userID + " AND comID = " + _comID + ") AND view = '1') AND moduleID = ";
                _strSQL += _ds.Tables[0].Rows[i]["id"].ToString() + ";";

                DataSet _dsForms = exec.getDataSet(_strSQL);

                if (_dsForms.Tables[0].Rows.Count > 0)
                {
                    if (_ds.Tables[0].Rows[i]["moduleName"].ToString() == "Administration") 
                    {
                        _menu = menuAdministration;
                    }
                    else if (_ds.Tables[0].Rows[i]["moduleName"].ToString() == "Fleet Management") 
                    {
                        _menu = menuFleet;
                    }
                    else
                    {
                        continue;
                    }

                    foreach (RadMenuItem item in _menu.Items)
                    {
                        _hasPermission = false;

                        for (int j = 0; j < _dsForms.Tables[0].Rows.Count; j++)
                        {
                            string _formName = _dsForms.Tables[0].Rows[j]["formName"].ToString();
                            if (_formName == item.Text)
                            {
                                _hasPermission = true;
                                break;
                            }
                        }

                        if (!_hasPermission && item.Value == "Admin") {
                            item.Visible = false;
                        }
                    }
                }
                else
                {
                    if (_ds.Tables[0].Rows[i]["moduleName"].ToString() == "Administration") 
                    {
                        menuAdministration.Visible = false;
                    }
                    else if (_ds.Tables[0].Rows[i]["moduleName"].ToString() == "Fleet Management") 
                    {
                        menuFleet.Items.Clear();
                    }
                }
            }
        }
    }
}
