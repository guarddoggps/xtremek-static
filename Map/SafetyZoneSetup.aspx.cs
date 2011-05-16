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
using AlarmasABC.BLL.ProcessSafetyZone;
using Artem.Web.UI.Controls;
using AlarmasABC.DAL.Queries;
using System.Text;

public partial class Tracking_SafetyZoneSetup : System.Web.UI.Page,ICallbackEventHandler
{
    static string geofenceID;
    string _str;

    int iconHeight = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconHeight"]);
    int iconWidth = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconWidth"]);


    protected void Page_Load(object sender, EventArgs e)
    {
     
        if (!IsPostBack)
        {
            ClientScriptManager m = Page.ClientScript;
            string str = m.GetCallbackEventReference(this, "args", "ReceiveServerData", "'this is context from server'");
            string strCallback = "function CallServer(args,context){" + str + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", strCallback, true);

            string _key = ConfigurationManager.AppSettings["gKey"].ToString();
            string _mapKey = "<script src='http://maps.google.com/maps?file=api&v=2&key=" + _key + "' type='text/javascript'></script>";
            Page.RegisterClientScriptBlock("", _mapKey);

            geofenceID = "";
            Session["unitID"] = null;
            string _unitID = Request.QueryString["unitID"].ToString();
            Session["unitID"] = _unitID;

            if (Session["geofenceID"] != null)
            {

                string ID = Session["geofenceID"].ToString();
                Console.WriteLine(ID);
                loadGeofence(ID);
                geofenceID = ID;
                Session["geofenceID"] = null;
                _Update.Value = "UPDATE";

            }
            else
            {
                loadGeofence("");
            }
        }

    }

    public string GetCallbackResult()
    {
        return _str.ToString();

    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        _str = GetMarkerText();
    }

    private string GetMarkerText()
    {
        string _iconName = "";
        double _lat, _lng;
        StringBuilder _bfMarkerScript = new StringBuilder();
        string _deviceID = getDeviceID(Request.QueryString["unitID"].ToString());

        try
        {
            string _sql = "SELECT lat,long FROM tblGPRS WHERE deviceID =" + _deviceID + "";
            _sql += "AND recTime= (SELECT MAX(recTime) FROM tblGPRS WHERE deviceID = " + _deviceID + ");";

            ExecuteSQL _exeSql = new ExecuteSQL();
            DataTable _dt = new DataTable();

            _dt = _exeSql.ExecuteSQLQuery(_sql);

            if (_dt.Rows.Count > 0)
            {
                _lng = double.Parse(_dt.Rows[0]["long"].ToString());
                _lat = double.Parse(_dt.Rows[0]["lat"].ToString());
                _iconName = getImageName();

                _bfMarkerScript.Append(" myMap.clearOverlays(); ");
                _bfMarkerScript.Append(" cPoint=new GPoint(" + _lng + "," + _lat + ");");
                _bfMarkerScript.Append(" var icon0 = new GIcon();");
                _bfMarkerScript.Append("icon0.image = \'../Icon/" + _iconName + ".png\';");
                _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                _bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");");
                _bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
                _bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");
                _bfMarkerScript.Append(" var marker=new GMarker(cPoint,icon0);");
                _bfMarkerScript.Append(" myMap.addOverlay(marker,icon0);");

                return _bfMarkerScript.ToString();
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

        return " myMap.clearOverlays(); ";
    }

    private void loadGeofence(string ID)
    {
        double _lat, _lng;
        string deviceID = getDeviceID(Request.QueryString["unitID"].ToString());
        try
        {
            string _sql = "SELECT lat,long FROM tblGPRS WHERE deviceID = " + deviceID +
                          " AND recTime= (SELECT MAX(recTime) FROM tblGPRS WHERE deviceID = " + deviceID + " AND direction = 0);";

            ExecuteSQL exeSql = new ExecuteSQL();
            DataTable _dt = new DataTable();

            _dt = exeSql.ExecuteSQLQuery(_sql);

            _lng = double.Parse(_dt.Rows[0]["long"].ToString());
            _lat = double.Parse(_dt.Rows[0]["lat"].ToString());

            string _name = "";
            string _Rad = "";

            ProcessSafetyZone _pGeoData = new ProcessSafetyZone();
            Geofence _geoInput = new Geofence();

            _geoInput.ComID = int.Parse(Session["trkCompany"].ToString());
            if (ID != "")
                _geoInput.Id = int.Parse(ID);
            _geoInput.UnitID = int.Parse(Request.QueryString["unitID"].ToString());
            _pGeoData.GeoInfo = _geoInput;
            _pGeoData.LoadGeoData();

            DataSet _ds = new DataSet();
            _ds = _pGeoData.Ds;

            StringBuilder bfMarker = new StringBuilder();
            StringBuilder bfMarkerScript = new StringBuilder();


                if (_ds.Tables[0].Rows.Count > 0)
                {
                    _name = _ds.Tables[0].Rows[0]["name"].ToString();
                    _Rad = _ds.Tables[0].Rows[0]["radius"].ToString();
                    _txtEmail.Text = _ds.Tables[0].Rows[0]["email"].ToString();

                    try
                    {
                        _txtPhoneNumber.Text = _ds.Tables[0].Rows[0]["phoneNumber"].ToString();
                        bool isSMS = bool.Parse(_ds.Tables[0].Rows[0]["isSMS"].ToString());
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
                    }
                    catch (Exception ex)
                    {
                        if (!_ckOnSMS.Checked)
                            _chkOffSMS.Checked = true;
                    }
                    
                    
                    

                    if (_ds.Tables[0].Rows.Count > 0)
                    {

                        if (_ds.Tables[0].Rows[0]["isActive"].ToString() == "True")
                            _rdoON.Checked = bool.Parse(_ds.Tables[0].Rows[0]["isActive"].ToString());
                        else 
                            _rdoOFF.Checked = true;
                    }

                    _txtZoneName.Text = _name;
                    _lblRadius.Text = _Rad;

                     double _radius = ((double.Parse(_Rad) / 1.60934) * 5280) / 3.2808399;


                    _Radius.Value = _ds.Tables[0].Rows[0]["Radius"].ToString();
                    _Lat.Value = _ds.Tables[0].Rows[0]["centerLat"].ToString();
                    _Long.Value = _ds.Tables[0].Rows[0]["centerLng"].ToString();

                    
                    string _imageName = getImageName();


                    bfMarker.Append("<script language=\"javascript\">");

                    bfMarker.Append("   myMap = new GMap2(document.getElementById('map')); ");
                    bfMarker.Append(" var rad = document.getElementById('_Radius');");
                    bfMarker.Append(" var gName = document.getElementById('_txtGeofenceName');");
                    bfMarker.Append(" var _Lat = document.getElementById('_Lat');");
                    bfMarker.Append(" var _Long = document.getElementById('_Long'); ");

                    bfMarker.Append(" myMap.addControl(new GSmallMapControl());");
                    bfMarker.Append(" myMap.addControl(new GMapTypeControl());");
                    bfMarker.Append(" myMap.enableScrollWheelZoom();");
                    bfMarker.Append(" myMap.setCenter(new GLatLng(_Lat.value,_Long.value),15,G_HYBRID_MAP);");
                    bfMarker.Append(" GEvent.addListener(myMap,'click',clickedMap);");

                    bfMarker.Append(" var _radius=((rad.value/1.60934)*5280)/3.2808399;  ");
                    bfMarker.Append(" createCircle(new GLatLng(_Lat.value,_Long.value),_radius);");
                    bfMarker.Append(" clear=true;");

                    bfMarkerScript.Append(" cPoint=new GLatLng(" + _lat + "," + _lng + ");");
                    bfMarkerScript.Append(" var icon0 = new GIcon();");
                    bfMarkerScript.Append(" icon0.image = \\'../Icon/" + _imageName + ".png\\';");
                    bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                    bfMarkerScript.Append(" icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");");
                    bfMarkerScript.Append(" icon0.infoWindowAnchor = new GPoint(15, 15);");
                    bfMarkerScript.Append(" icon0.infoShadowAnchor = new GPoint(18, 25);");
                    bfMarkerScript.Append(" var marker=new GMarker(cPoint,icon0);");
                    
                    bfMarkerScript.Append(" myMap.addOverlay(marker,icon0);");

                    bfMarker.Append(" setTimeout('evalMarker(\"" + bfMarkerScript.ToString() + "\")',1000); ");
                    bfMarker.Append("</script>");
                    Page.RegisterStartupScript("marker", bfMarker.ToString());

             }


            else
            {
                try
                {
                    string _iconName = getImageName();

                    bfMarker.Append("<script language=\"javascript\">");

                    bfMarker.Append("   myMap = new GMap2(document.getElementById('map')); ");
                    bfMarker.Append(" myMap.addControl(new GSmallMapControl()); ");
                    bfMarker.Append(" myMap.addControl(new GMapTypeControl()); ");
                    bfMarker.Append(" myMap.enableScrollWheelZoom();");
                    bfMarkerScript.Append(" myMap.setCenter(new GLatLng(" + _lat + "," + _lng + "),14,G_HYBRID_MAP);");
                    bfMarker.Append(" GEvent.addListener(myMap,'click',clickedMap);");


                    bfMarkerScript.Append(" cPoint=new GLatLng(" + _lat + "," + _lng + ");");
                    bfMarkerScript.Append(" var icon0 = new GIcon();");
                    bfMarkerScript.Append(" icon0.image = \\'../Icon/" + _iconName + ".png\\';");
                    bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                    bfMarkerScript.Append(" icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");");
                    bfMarkerScript.Append(" icon0.infoWindowAnchor = new GPoint(15, 15);");
                    bfMarkerScript.Append(" icon0.infoShadowAnchor = new GPoint(18, 25);");
                    bfMarkerScript.Append(" var marker=new GMarker(cPoint,icon0);");
                    

                    bfMarkerScript.Append(" myMap.addOverlay(marker);");

                    bfMarker.Append(" setTimeout('evalMarker(\"" + bfMarkerScript.ToString() + "\")',100); ");
                    bfMarker.Append("</script>");
                    Page.RegisterStartupScript("marker", bfMarker.ToString());

                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }



            }
        }
        catch (Exception ex)
        {
            throw new Exception("loadGeofence(): " + ex.Message.ToString());

        }
     
    }

    private string getImageName()
    {
        string deviceID = getDeviceID(Request.QueryString["unitID"].ToString());
        string _imageName ="";
        string _comID = "";
        try
        {
            _comID = Session["trkCompany"].ToString();

        string _strSQL = "SELECT * FROM unitRecords WHERE deviceID=" + deviceID + 
                         " AND recTime = (SELECT MAX(recTime) FROM unitRecords" +
                         " WHERE deviceID = " + deviceID + ") AND comID =" + _comID + ";";
       

        DataTable table = new DataTable();
        ExecuteSQL exSQl = new ExecuteSQL();
        
        table = exSQl.ExecuteSQLQuery(_strSQL);
            if(table.Rows.Count>0)
            {
             _imageName = table.Rows[0]["iconName"].ToString();
            }
        }
        catch (Exception ex)
        {
            return _imageName;
        }

        return _imageName;
    }
      
    
    protected void _btnCancel_Click(object sender, EventArgs e)
    {
        string _unitID = Request.QueryString["unitID"].ToString();
        Response.Redirect("../Tracking/SafetyZones.aspx?unitID=" + _unitID + "");
    }

    protected void _btnOK_Click(object sender, EventArgs e)
    {
        CreateUpdateGeofence();
    }

    private void CreateUpdateGeofence()
    {
        if (_Radius.Value == "")
        {
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Please define a geofence ";
            return;
        }
        decimal _radius = decimal.Parse(_Radius.Value);
        
        if (validInput(_radius))
        {
            if (_Update.Value == "UPDATE")
            {
                
                updateGeofence();
            }
            else
            {
                saveGeofence();
            }

        }

    }

    private void saveGeofence()
    {
        try
        {
            GoogleLocation _location = GoogleLocation.Parse(_centerPoint.Value);
            Geofence _geoObj = new Geofence();
             
            ProcessSafetyZone _safetyZone = new ProcessSafetyZone(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

            _geoObj.ComID = int.Parse(Session["trkCompany"].ToString());
            _geoObj.Name = _txtZoneName.Text.ToString();
            _geoObj.Email = _txtEmail.Text.ToString();
            _geoObj.UnitID = int.Parse(Request.QueryString["unitID"].ToString());
            _geoObj.CenterLat= decimal.Parse(_location.Latitude.ToString());
            _geoObj.CenterLong= decimal.Parse(_location.Longitude.ToString());
            _geoObj.Radius = decimal.Parse(_Radius.Value);
            _geoObj.IsActive = _rdoON.Checked;
            _geoObj.SpeedingPhoneNum = _txtPhoneNumber.Text.Trim();
            _geoObj.IsSMS = _ckOnSMS.Checked;

            _safetyZone.GeoInfo = _geoObj;
            _safetyZone.invoke();

            _lblMessage.ForeColor = System.Drawing.Color.Green;
            _lblMessage.Text = "Geofence created successfully ";
        }
        catch (Exception ex)
        {
                Console.WriteLine(ex.Message.ToString());
                _lblMessage.ForeColor = System.Drawing.Color.Red;
                _lblMessage.Text = "Try Agaim ";

                throw new Exception(ex.Message.ToString());
            
        }
    }

    private void updateGeofence()
    {
        try
        {
            GoogleLocation _location = GoogleLocation.Parse(_centerPoint.Value);
            Geofence _geoObj = new Geofence();

            ProcessSafetyZone _safetyZoneUpdate = new ProcessSafetyZone(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
            _geoObj.Id = int.Parse(geofenceID);
            _geoObj.ComID = int.Parse(Session["trkCompany"].ToString());
            _geoObj.Name = _txtZoneName.Text.ToString();
            _geoObj.Email = _txtEmail.Text.ToString();
            _geoObj.UnitID = int.Parse(Request.QueryString["unitID"].ToString());
            _geoObj.CenterLat = decimal.Parse(_location.Latitude.ToString());
            _geoObj.CenterLong = decimal.Parse(_location.Longitude.ToString());
            _geoObj.Radius = decimal.Parse(_Radius.Value);
            _geoObj.IsActive = _rdoON.Checked;
            _geoObj.SpeedingPhoneNum = _txtPhoneNumber.Text.Trim();
            _geoObj.IsSMS = _ckOnSMS.Checked;


            _safetyZoneUpdate.GeoInfo = _geoObj;
            _safetyZoneUpdate.invoke();

            _lblMessage.ForeColor = System.Drawing.Color.Green;
            _lblMessage.Text = "Geofence update successfully ";
            _Update.Value = "";



        }
        catch (Exception ex)
        {
            ex.Message.ToString();

            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Geofence update fail";


        }
    }

    private bool validInput(decimal _radius)
    {
        if (_txtZoneName.Text == "Enter Zone Name" || _txtZoneName.Text == "")
        {
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Please Enter Geofence name";
            
            return false;
        }
        else if (_radius == 0)
        {
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Radius can't be 0";
           
            return false;
        }
        else if (_txtEmail.Text == "")
        {
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            _lblMessage.Text = "Invalid Email";
            
            return false;
        }


        return true;
    }

    protected void _btnClear_Click(object sender, EventArgs e)
    {
        
    }

    public string getMarker()
    {
        StringBuilder bfMarker = new StringBuilder();
        StringBuilder bfMarkerScript = new StringBuilder();
        //bfMarker.AppendFormat("{0}.clearOverlays();", );

        try
        {
            string unitID = Session["unitID"].ToString();
            string deviceID = getDeviceID(unitID);
            string _comID = Session["trkCompany"].ToString();
            GoogleMarker _currentMarker = new GoogleMarker();

            double _lat = 0, _lng = 0;

            DataTable _dt = new DataTable();
            

            string _strSQL = "SELECT unitName,lat,long,deviceID,city,state,country,CAST(velocity*0.621 AS INT) AS velocity," +
                             "recTimeRevised,recTime,iconName FROM vwUnitRecords WHERE recTime IN " +
                             "(SELECT MAX(recTime) FROM vwUnitRecords WHERE deviceID IN (SELECT deviceID FROM " +
                             "vwUserWiseUnits WHERE uID = " + Session["trkUID"].ToString() + " AND deviceID = " + deviceID + 
                             ") GROUP BY deviceID) AND comID = " + _comID + " AND deviceID = " + deviceID + ";";
            

            ExecuteSQL _exeSQL = new ExecuteSQL();
            _dt = _exeSQL.ExecuteSQLQuery(_strSQL);



            if (_dt.Rows.Count > 0)
            {
                _lat = double.Parse(_dt.Rows[0]["lat"].ToString());
                _lng = double.Parse(_dt.Rows[0]["long"].ToString());
                bfMarker.Append("<script language=\"javascript\">");

               
                //}
               
                if (_dt.Rows[0]["iconName"].ToString() != "")
                {
                    bfMarkerScript.Append("cPoint=new GPoint(" + JsUtil.Encode(_lng) + "," + JsUtil.Encode(_lat) + ");");
                    bfMarkerScript.Append(" var icon0 = new GIcon();");
                    bfMarker.Append("icon0.image = '../Icon/" + _dt.Rows[0]["iconName"].ToString() + ".png';");
                    bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                    bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");");
                    bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
                    bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");
                    bfMarkerScript.Append(" var marker=new GMarker(cPoint,icon0);");

                }
                bfMarkerScript.Append("myMap=new GMap2(document.getElementById(\\'map\\'));");
                bfMarkerScript.Append("myMap.setCenter(new GLatLng(" + _lat + "," + _lng + "),5,G_HYBRID_MAP);");

                bfMarkerScript.Append(" myMap.addOverlay(marker);");

                bfMarker.Append(" setTimeout('evalMarker(\"" + bfMarkerScript.ToString() + "\")',100); ");
                bfMarker.Append("</script>");
                Page.RegisterStartupScript("marker", bfMarker.ToString());


            }

        }
        catch (Exception ex)
        {

        }
        return bfMarker.ToString();
    }

    protected void _ckOnSMS_CheckedChanged(object sender, EventArgs e)
    {
        _chkOffSMS.Checked = false;
    }
    protected void _chkOffSMS_CheckedChanged(object sender, EventArgs e)
    {
        _ckOnSMS.Checked = false;
    }

    private string getDeviceID(string unitID)
    {
        try
        {
            string _strSQL = "SELECT deviceID FROM tblUnits WHERE unitID = " + unitID + ";";
            DataTable _dt = new DataTable();
            //Database _db = new Database();

            ExecuteSQL _exSQl = new ExecuteSQL();
            _dt=  _exSQl.ExecuteSQLQuery(_strSQL);
           

            if (_dt.Rows.Count > 0)
            {
                return _dt.Rows[0]["deviceID"].ToString();
            }
            else
                return "";
        }
        catch (Exception ex)
        {
            return "";
        }
    }
}
