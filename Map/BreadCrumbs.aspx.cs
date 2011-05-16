using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessMapData;

public partial class Map_BreadCrumbs : System.Web.UI.Page
{
    static double latInc = 0;
    static double lngInc = 0;
    double focussingLat = 0;
    double focussingLong = 0;
    double prevLat = 0;
    double prevLng = 0;
    static int markerCount = 0;
    static int RecTime = 0;

    int iconHeight = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconHeight"]);
    int iconWidth = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconWidth"]);


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Unit"] != null)
            {
                if (Session["Unit"].ToString() != Request.QueryString["deviceID"].ToString())
                {
                    Session["Unit"] = null;
                    Session["recTime"] = null;
                    Session["_zoomLevel"] = null;
                }
            }

            Session["recTime"] = null;

            string _key = ConfigurationManager.AppSettings["gKey"].ToString();
            string _mapKey = "<script src='http://maps.google.com/maps?file=api&v=2&key=" + _key + "' type='text/javascript'></script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", _mapKey.ToString(), false);

            loadInitialMarkerData();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        if (IsPostBack)
        {
          	ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_Refresh", GetMarkerScript(false), true);
        }
    }

	private string GetMarkerScript(bool init)
	{
     	StringBuilder _bfScript = new StringBuilder();
       	StringBuilder _bfMarkerScript = new StringBuilder();
        StringBuilder _bfPolyLine = new StringBuilder();
        StringBuilder _script = new StringBuilder();

        IList<MapData> _mapData;
        ProcessBreadCrumbsData _breadCrumbs = new ProcessBreadCrumbsData();
           
        try
        {
         	if (Session["recTime"] != null)
          	{
            	_breadCrumbs.ComID = int.Parse(Session["trkCompany"].ToString());
              	_breadCrumbs.DeviceID = int.Parse(Request.QueryString["deviceID"].ToString());
            	_breadCrumbs.RecTime = long.Parse(Session["recTime"].ToString());
             	_breadCrumbs.GetData1();
            	_mapData = _breadCrumbs.MapData;
                _bfScript.Append(" map.clearOverlays();"); 
               	_bfScript.Append(" DisPoseNodes(); ");

			}
			else
            {
				if (!init)
					return null;

		      	_breadCrumbs.ComID = int.Parse(Session["trkCompany"].ToString());
		      	_breadCrumbs.DeviceID = int.Parse(Request.QueryString["deviceID"].ToString());
		       	_breadCrumbs.GetData2();
		     	_mapData = _breadCrumbs.MapData;
          	}

				if (_mapData.Count > 0)
		        {
		            string _markerText = "";
		            decimal _lat = 0, _lng = 0;
                                    
                   	_bfPolyLine.Append(" polyline=new GPolyline([ ");

		            for (int i = 0; i < _mapData.Count; i++)
		            {
						AlertData _alert = new AlertData();
						MapDataCommon.GetAlert(ref _alert, Session["trkCompany"].ToString(),
						                 	   _mapData[i].DeviceID, _mapData[i].RecTimeRevised);    

		                // Get the marker text for the map data object
		                _markerText = MapDataCommon.GetMapMarkerText(_mapData[i], _alert.AlertMessage.Replace("\"", @"\\"""));

						if (init)
						{
						    if (Session["recTime"] == null)
						   	{
						      	Session["recTime"] = long.Parse(_mapData[i].RecTime.ToString());
						   	}
						}

                        _bfPolyLine.Append(" new GLatLng(" + _mapData[i].Latitude + "," + _mapData[i].Longitude + "), ");

		                _bfMarkerScript.Append("  point=new GPoint(" + _mapData[i].Longitude + "," + _mapData[i].Latitude + ");");
		                
		                if (i == 0)
		                {         
		                    _bfMarkerScript.Append(" icon0 = new GIcon();");
		                    _bfMarkerScript.Append(" icon0.image = \\'../Icon/" + _mapData[i].IconName + ".png\\';");
		                    _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight  + "," + iconWidth  + ");");
		                    _bfMarkerScript.Append(" icon0.iconAnchor = new GPoint(" + iconHeight/2 + "," + iconWidth/2 + ");");
		                    _bfMarkerScript.Append(" icon0.infoWindowAnchor = new GPoint(15, 15);");
		                    _bfMarkerScript.Append(" icon0.infoShadowAnchor = new GPoint(18, 25);");
							_bfMarkerScript.Append(" label = new ELabel(new GLatLng(" + _mapData[i].Latitude + "," + _mapData[i].Longitude + "), \\'" + _mapData[i].UnitName + "\\',\\'tag_red\\');");
						    _bfMarkerScript.Append(" label.pixelOffset=new GSize(15,0);");
						    _bfMarkerScript.Append(" map.addOverlay(label);");
							_bfMarkerScript.Append(" if (hide) label.hide();");
		                    _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");

		                }
						else
						{
							string color = MapDataCommon.GetAlertColor(_alert);
							_bfMarkerScript.Append(" icon0 = new GIcon();");
				            _bfMarkerScript.Append(" icon0.image = \\'../Images/crumb_" + color + ".png\\';");
				            _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + 20  + "," + 34  + ");");
				            _bfMarkerScript.Append(" icon0.iconAnchor = new GPoint(9,34);");
				            _bfMarkerScript.Append(" icon0.infoWindowAnchor = new GPoint(9,9);");
                            _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");

						}
		                
		                _bfMarkerScript.Append(" myEvent[" + i + "]= GEvent.addListener(marker[" + i + "],\\'click\\',function(){");
		                _bfMarkerScript.Append(" ");
		                _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(\\'" + _markerText + "\\'); });");
		                _bfMarkerScript.Append("map.addOverlay(marker[" + i + "]);");

		                _lat = _mapData[i].Latitude;
		                _lng = _mapData[i].Longitude;

		                if (_mapData[i].Velocity == "0")
		                {
		                    i = _mapData.Count;
		                }

		            }


                    _bfPolyLine.Append(" new GLatLng(" + _lat + "," + _lng + ")], ");
                    _bfPolyLine.Append(" \\'#008000\\', 10);");
                    _bfPolyLine.Append("map.addOverlay(polyline)");

                    _bfMarkerScript.Append(_bfPolyLine.ToString()); 

					if (init)
			            _script.Append("map.setCenter(new GLatLng(" + _lat + "," + _lng + "),13,G_HYBRID_MAP);");
					else
						_script.Append("map.setCenter(new GLatLng(" + _mapData[0].Latitude + "," + _mapData[0].Longitude + "));");
		            _script.Append(_bfMarkerScript.ToString());

		            _bfScript.Append(" setTimeout('evalMarker(\"" + _script.ToString() + "\")',5); ");

					return _bfScript.ToString();
			}
		}
		catch (Exception ex)
       	{
			Console.WriteLine(ex.Message);
     	}
    	finally
     	{
        	_mapData = null;
          	_bfMarkerScript = null;
          	_bfPolyLine = null;
          	_bfScript = null;
          	_breadCrumbs = null;
       	}

		return "";
            
	}
	

    private void loadInitialMarkerData()
    {
        Session["Unit"] = Request.QueryString["deviceID"].ToString();

        try
        {
			// Register the map first
			string _mapScript = "";
            _mapScript += " map=new GMap2(document.getElementById('Map'));";
            _mapScript += " map.enableScrollWheelZoom();";
            _mapScript += " map.addControl(new GLargeMapControl());";
            _mapScript += " map.addControl(new GMapTypeControl());";
            _mapScript += " map.setCenter(new GLatLng(43.907787,-79.359741),2,G_HYBRID_MAP);";
			ClientScript.RegisterStartupScript(this.GetType(), "map", _mapScript, true);

			string str = GetMarkerScript(true);
            ClientScript.RegisterStartupScript(this.GetType(), "marker", str, true);
            
        }
        catch (Exception ex)
        {
            throw new Exception(" AlarmasABC::Map::breadCrumbs :: " + ex.Message.ToString());
        }
        finally
        {
        }
    
    }
}
