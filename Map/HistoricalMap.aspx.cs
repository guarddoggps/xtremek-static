using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessMapData;
using AlarmasABC.DAL.Queries;

public partial class Map_Historical : System.Web.UI.Page,ICallbackEventHandler
{
    string _str="";
    int iconHeight = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconHeight"]);
    int iconWidth = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconWidth"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (AMS.Web.RemoteScripting.InvokeMethod(Page))
            return; 

        if (!IsPostBack)
        {
            ClientScriptManager m = Page.ClientScript;
            string str = m.GetCallbackEventReference(this, "args", "ReceiveServerData", "'this is context from server'");
            string strCallback = "function CallServer(args,context){" + str + ";}";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", strCallback, true);

            string _key = ConfigurationManager.AppSettings["gKey"].ToString();
            string _mapKey = "<script src='http://maps.google.com/maps?file=api&v=2&key=" + _key + "' type='text/javascript'></script>";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "", _mapKey, false);

            DrawMarker();	
        }
    }

    public string GetCallbackResult()
    {
        return _str;

    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        _str = GetMarkerScript();
		// This is so that the callback script will be correct
		_str = _str.Replace("'", "\'");
    }

	private void DrawMarker()
    {
		string mapScript = "";
		string _script, _bfScript;

		// First create the map so that if there is a problem loading the
		// data at least it shows something nice and not that horrid white
		// screen of terror....
		mapScript += " var mapDiv = document.getElementById('Map');";
		mapScript += " map=new GMap2(mapDiv);";
        mapScript += " map.enableScrollWheelZoom();";
        mapScript += " map.addControl(new GLargeMapControl());";
        mapScript += " map.addControl(new GMapTypeControl());";	
        mapScript += " map.setCenter(new GLatLng(43.907787,-79.359741),11,G_HYBRID_MAP);";
		ClientScript.RegisterStartupScript(this.GetType(), "map", mapScript, true);

		// Now get the data
        _script =  "function GetMapData() {";
		_script += GetMarkerScript();
		_script += "}";
		_bfScript = _script;
        //_bfScript += " setTimeout('evalMarker(GetMapData())',5); ";
		_bfScript += " evalMarker(GetMapData());";

		ClientScript.RegisterStartupScript(this.GetType(), "marker", _bfScript, true);
    }

	public string GetMarkerScript()
    {
        DataSet ds = null;
        string _bfMarkerScript = "";
        string _script = "";
        string _bfPolyLine = "";
        int end = 0;
        int start = 0;
        int _labelID = 0;
        

        try
        {
            ds = (DataSet)Session["historicalData"];

            if (ds.Tables[0].Rows.Count> 0)
            {                

                string _markerText = "";
                decimal _lat = 0, _lng = 0;
				decimal latest_lat = 0, latest_lon = 0;

                start = int.Parse(Session["historicalPageID"].ToString()) * 25;
                end = start + 25;

                if (ds.Tables[0].Rows.Count < 25 || end > ds.Tables[0].Rows.Count) 
				{
                    end = ds.Tables[0].Rows.Count;
				}

				latest_lat = decimal.Parse(ds.Tables[0].Rows[start]["lat"].ToString());
				latest_lon = decimal.Parse(ds.Tables[0].Rows[start]["long"].ToString());

                _bfPolyLine += " var polyline=new GPolyline([ ";

                for (int i = start; i < end; i++)
                {
					int offset = i - start;
					
                    AlertData _alert = new AlertData();
                    MapDataCommon.GetAlert(ref _alert, Session["trkCompany"].ToString(),
                             ds.Tables[0].Rows[i]["deviceID"].ToString(), 
                             ds.Tables[0].Rows[i]["recDateTime"].ToString());

					_markerText = MapDataCommon.GetDataSetMarkerText(ds, i, _alert.AlertMessage);

                    _bfMarkerScript += " point=new GPoint(" + decimal.Parse(ds.Tables[0].Rows[i]["long"].ToString()) + "," +decimal.Parse(ds.Tables[0].Rows[i]["Lat"].ToString()) + ");";                    

                    _bfPolyLine += " new GLatLng(" + decimal.Parse(ds.Tables[0].Rows[i]["lat"].ToString()) + "," + decimal.Parse(ds.Tables[0].Rows[i]["Long"].ToString()) + "), ";

                    if(i==0)
                    {
                    
                        _bfMarkerScript += " icon0 = new GIcon();";
                        _bfMarkerScript += " icon0.image = '../Icon/" + ds.Tables[0].Rows[i]["iconName"].ToString() + ".png';";
                        _bfMarkerScript += " icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");";
                        _bfMarkerScript += " icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");";
                        _bfMarkerScript += " icon0.infoWindowAnchor = new GPoint(15, 15);";
                        _bfMarkerScript += " marker[" + offset + "]=new GMarker(point,icon0);";

                    }  
                    else
                    {
                        string color = MapDataCommon.GetAlertColor(_alert);
	
                        _bfMarkerScript += " icon0 = new GIcon();";
                        _bfMarkerScript += " icon0.image = '../Images/crumb_" + color + ".png';";
                        _bfMarkerScript += " icon0.iconSize = new GSize(" + 20  + "," + 34  + ");";
                        _bfMarkerScript += " icon0.iconAnchor = new GPoint(9,34);";
                        _bfMarkerScript += " icon0.infoWindowAnchor = new GPoint(9,9);";
                        _bfMarkerScript += " marker[" + offset + "]=new GMarker(point,icon0);";
                    }

                    _bfMarkerScript += "label[" + offset + "] = new ELabel(new GLatLng(" + decimal.Parse(ds.Tables[0].Rows[i]["Lat"].ToString()) + "," + 
									  decimal.Parse(ds.Tables[0].Rows[i]["long"].ToString()) + "), '" + 
									 (ds.Tables[0].Rows.Count - i).ToString() + "', 'style1');";
                    _bfMarkerScript += "label[" + offset + "].pixelOffset=new GSize(5,10);";
                    _bfMarkerScript += "map.addOverlay(label[" + offset + "]);";
                    
                    _bfMarkerScript += " myEvent[" + offset + "] = GEvent.addListener(marker[" + offset + "],'click',function(){";
                    _bfMarkerScript += " ";
                    _bfMarkerScript += " marker[" + offset + "].openInfoWindowHtml('" + _markerText + "'); });";
                    _bfMarkerScript += "map.addOverlay(marker[" + offset + "]);";
                    _bfMarkerScript += " gmarkers[" + offset + "] = marker[" + offset + "];";
                    _bfMarkerScript += "htmls[" + offset + "] = '" + _markerText + "';";
                }
				
				
   				_lat = decimal.Parse(ds.Tables[0].Rows[end - 1]["lat"].ToString());
    			_lng = decimal.Parse(ds.Tables[0].Rows[end - 1]["long"].ToString());
				
				_bfPolyLine += " new GLatLng(" + _lat + "," + _lng + ")], ";
                _bfPolyLine += " '#008000', 10);";
                _bfPolyLine += "map.addOverlay(polyline);";

                _bfMarkerScript += _bfPolyLine;

        		_script += " map.clearOverlays();";
                _script += " map.setCenter(new GLatLng(" + latest_lat + "," + latest_lon + "),11,G_HYBRID_MAP);";
                _script += " GEvent.addListener(map,'zoomend',afterZoomEnd);";
                _script += _bfMarkerScript.ToString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

		return _script;
    }
   
}
