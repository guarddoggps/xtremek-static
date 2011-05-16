using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Threading;
using Artem.Web.UI.Controls;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessMapData;
using System.Timers;


public partial class Map_MainMap : System.Web.UI.Page,ICallbackEventHandler
{
    int iconHeight = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconHeight"]);
    int iconWidth = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconWidth"]);
    string _str = "";

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

            if (Session["trkCompany"] != null && Session["uID"] != null)
            {
                if (Request.Params["tID"] != null)
                {
                    ProcessData(int.Parse(Session["trkCompany"].ToString()), int.Parse(Session["uID"].ToString()), int.Parse(Request.Params["tID"].ToString()));
                }
                else
                {
                    ProcessData(int.Parse(Session["trkCompany"].ToString()), int.Parse(Session["uID"].ToString()));
                }
            }
        }
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        if (e.Argument == "InitialPageLoad")
        {
            Panel2.Visible = false;
        }
    }

    public string GetCallbackResult()
    {
        return _str.ToString();

    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        _str = GetMarkerScript();
    }

    private void ProcessData(int _comID,int _userID)
    {
        StringBuilder _bfScript = new StringBuilder();
        StringBuilder _bfMarkerScript = new StringBuilder();
        StringBuilder _script = new StringBuilder();
        

        IList<MapData> _mapData;
        ProcessMainMapData _processMapData = new ProcessMainMapData(0);
        _processMapData.ComID = _comID;
        _processMapData.UserID = _userID;

        try
        {
            _processMapData.invoke();
            _mapData = _processMapData.MapData;
            _processMapData = null;


            if (_mapData.Count > 0)
            {

                string _markerText = "";
                decimal _lat = 0, _lng = 0;

                _bfScript.Append("<script language=\"javascript\">");
               // _bfScript.Append(" disPoseEvent(); ");


                for (int i = 0; i < _mapData.Count; i++)
                {
                    // Get the marker text for the map data object
                    _markerText = MapDataCommon.GetMapMarkerText(_mapData[i]);

                    _bfMarkerScript.Append("  point=new GPoint(" + _mapData[i].Longitude + "," + _mapData[i].Latitude + ");");
                    _bfMarkerScript.Append(" icon0 = new GIcon();");
                    _bfMarkerScript.Append("icon0.image = \\'../Icon/" + _mapData[i].IconName + ".png\\';");
                    _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight  + "," + iconWidth  + ");");
                    _bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");");
                    _bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
                    _bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");
		      		_bfMarkerScript.Append("label[" + i + "] = new ELabel(new GLatLng(" + _mapData[i].Latitude + "," + _mapData[i].Longitude + "), \\'" + _mapData[i].UnitName + "\\', \\'tag_red\\');");
                    _bfMarkerScript.Append("label[" + i + "].pixelOffset=new GSize(5,10);");
                    _bfMarkerScript.Append("map.addOverlay(label[" + i + "]);");



                    _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");
                    _bfMarkerScript.Append(" myEvent[" + i + "]=GEvent.addListener(marker[" + i + "],\\'click\\',function(){");
                    _bfMarkerScript.Append(" ");
                    _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(\\'" + _markerText.Trim() + "\\'); });");
                    _bfMarkerScript.Append("map.addOverlay(marker[" + i + "]);");

                    _lat = _mapData[i].Latitude;
                    _lng = _mapData[i].Longitude;
                }

                _script.Append("map=new GMap2(document.getElementById(\\'Map\\'));");
                _script.Append("map.enableScrollWheelZoom();");
                _script.Append("map.addControl(new GLargeMapControl());");
                _script.Append("map.addControl(new GMapTypeControl());");
                _script.Append("map.setCenter(new GLatLng(" + _lat + "," + _lng + "),4,G_HYBRID_MAP);");
                _script.Append("map.clearOverlays();");
                _script.Append("GEvent.addListener(map,\\'zoomend\\',afterZoomEnd);");
                _script.Append(_bfMarkerScript.ToString());

                _bfScript.Append(" setTimeout('eval(\"" + _script.ToString() + "\")',5); ");
                _bfScript.Append("</script>");
                Page.RegisterStartupScript("marker", _bfScript.ToString());
            }
        }

        catch (Exception ex)
        {
            throw new Exception(" AlarmasABC::MainMap::" + ex.Message);
        }
        finally
        {
            _mapData = null;
            _processMapData = null;
        }
    }

    private void ProcessData(int _comID, int _userID, int _typeID)
    {
        StringBuilder _bfScript = new StringBuilder();
        StringBuilder _bfMarkerScript = new StringBuilder();
        StringBuilder _script = new StringBuilder();


        IList<MapData> _mapData;
        ProcessMainMapData _processMapData = new ProcessMainMapData(1);
        _processMapData.ComID = _comID;
        _processMapData.UserID = _userID;
        _processMapData.TypeID = _typeID;

        try
        {
            _processMapData.invoke();
            _mapData = _processMapData.MapData;

            string _markerText = "";
            decimal _lat = 0, _lng = 0;

            _bfScript.Append("<script language=\"javascript\">");
            //_bfScript.Append(" disPoseEvent(); ");

            for (int i = 0; i < _mapData.Count; i++)
            {
                // Get the marker text for the map data object
                _markerText = MapDataCommon.GetMapMarkerText(_mapData[i]);

                _bfMarkerScript.Append("info[" + i + "] = \\'"+_markerText+"\\';");
                _bfMarkerScript.Append("  point=new GPoint(" + _mapData[i].Longitude + "," + _mapData[i].Latitude + ");");
                _bfMarkerScript.Append(" icon0 = new GIcon();");
                _bfMarkerScript.Append("icon0.image = \\'../Icon/" + _mapData[i].IconName + ".png\\';");
                _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                _bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight/2 + "," + iconWidth/2 + ");");
                _bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
                _bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");
		      	_bfMarkerScript.Append("label[" + i + "] = new ELabel(new GLatLng(" + _mapData[i].Latitude + "," + _mapData[i].Longitude + "), \\'" + _mapData[i].UnitName + "\\', \\'tag_red\\');");
                _bfMarkerScript.Append("label[" + i + "].pixelOffset=new GSize(5,10);");
                _bfMarkerScript.Append("map.addOverlay(label[" + i + "]);");


                _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");
                _bfMarkerScript.Append(" myEvent[" + i + "]=GEvent.addListener(marker[" + i + "],\\'click\\',function(){");
                _bfMarkerScript.Append(" ");
                _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(info[" + i + "]); });");
                _bfMarkerScript.Append("map.addOverlay(marker[" + i + "]);");

                _lat = _mapData[i].Latitude;
                _lng = _mapData[i].Longitude;
            }

            _script.Append("map=new GMap2(document.getElementById(\\'Map\\'));");
            _script.Append("map.enableScrollWheelZoom();");
            _script.Append("map.addControl(new GLargeMapControl());");
            _script.Append("map.addControl(new GMapTypeControl());");
            _script.Append("map.setCenter(new GLatLng(" + _lat + "," + _lng + "),4,G_HYBRID_MAP);");
            _script.Append("map.clearOverlays();");
            _script.Append("GEvent.addListener(map,\\'zoomend\\',afterZoomEnd);");
            _script.Append(_bfMarkerScript.ToString());

            _bfScript.Append(" setTimeout('evalMarker(\"" + _script.ToString() + "\")',5); ");
            _bfScript.Append("</script>");
			Page.RegisterStartupScript("marker", _bfScript.ToString());

        }
        catch (Exception ex)
        {
        	Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            _mapData = null;
            _processMapData = null;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (IsPostBack)
        {
            if (Session["trkCompany"] != null && Session["uID"] != null)
            {
                if (Request.Params["tID"] != null)
                {
                    try
                    {
                        StringBuilder _bfScript = new StringBuilder();
                        StringBuilder _bfMarkerScript = new StringBuilder();
                        StringBuilder _script = new StringBuilder();


                        IList<MapData> _mapData;
                        ProcessMainMapData _processMapData = new ProcessMainMapData(1);
                        _processMapData.ComID = int.Parse(Session["trkCompany"].ToString());
                        _processMapData.UserID = int.Parse(Session["uID"].ToString());
                        _processMapData.TypeID = int.Parse(Request.Params["tID"].ToString());

                        try
                        {
                            _processMapData.invoke();
                            _mapData = _processMapData.MapData;

                            string _markerText = "";
                            decimal _lat = 0, _lng = 0;

                            _bfScript.Append("<script language=\"javascript\">");
                            _bfScript.Append(" disPoseEvent(); ");

                            for (int i = 0; i < _mapData.Count; i++)
                            {
                                // Get the marker text for the map data object
                                _markerText = MapDataCommon.GetMapMarkerText(_mapData[i]);

                                _bfMarkerScript.Append(" if (point!=null){point=null;}");
                                _bfMarkerScript.Append(" point=new GPoint(" + _mapData[i].Longitude + "," + _mapData[i].Latitude + ");");
                                _bfMarkerScript.Append("if(icon0!=null){icon0=null;}");
                                _bfMarkerScript.Append(" icon0 = new GIcon();");
                                _bfMarkerScript.Append("icon0.image = \\'../Icon/" + _mapData[i].IconName + ".png\\';");
                                _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                                _bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight/2 + "," + iconWidth/2 + ");");
                                _bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
                                _bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");
		      					_bfMarkerScript.Append("label[" + i + "] = new ELabel(new GLatLng(" + _mapData[i].Latitude + "," + _mapData[i].Longitude + "), \\'" + _mapData[i].UnitName + "\\', \\'tag_red\\');");
                				_bfMarkerScript.Append("label[" + i + "].pixelOffset=new GSize(5,10);");
                				_bfMarkerScript.Append("map.addOverlay(label[" + i + "]);");


                                _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");
                                _bfMarkerScript.Append(" myEvent[" + i + "]=GEvent.addListener(marker[" + i + "],\\'click\\',function(){");
                                _bfMarkerScript.Append(" ");
                                _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(\\'" + _markerText.Trim() + "\\'); });");
                                _bfMarkerScript.Append("map.addOverlay(marker[" + i + "]);");

                                _lat = _mapData[i].Latitude;
                                _lng = _mapData[i].Longitude;
                            }
                                                      
                           
                            _script.Append("map.clearOverlays();");
                            _script.Append(" zoomEvent = GEvent.addListener(map,\\'zoomend\\',afterZoomEnd);");
                           
                            _script.Append("document.getElementById(\\'_Zoom\\').value=map.getZoom().toString();");
                            _script.Append("afterZoomEnd();");                            

                            _script.Append(_bfMarkerScript.ToString());
                            
                            _bfScript.Append("eval(\"" + _script.ToString() + "\"); ");
                            _bfScript.Append("</script>");

                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "_RefreshMainMap", _bfScript.ToString(), false);

                        }
                        catch (Exception ex)
                        {
                            throw new Exception("" + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                		Console.WriteLine(ex.Message.ToString());
                    }
                }

                else
                {
                    try
                    {

                        StringBuilder _bfScript = new StringBuilder();
                        StringBuilder _bfMarkerScript = new StringBuilder();
                        StringBuilder _script = new StringBuilder();


                        IList<MapData> _mapData;
                        ProcessMainMapData _processMapData = new ProcessMainMapData(0);

                        _processMapData.ComID = int.Parse(Session["trkCompany"].ToString());
                        _processMapData.UserID = int.Parse(Session["uID"].ToString());                       


                        try
                        {
                            _processMapData.invoke();
                            _mapData = _processMapData.MapData;

                            if (_mapData.Count > 0)
                            {

                                string _markerText = "";
                                decimal _lat = 0, _lng = 0;

                                _bfScript.Append("<script language=\"javascript\">");
                                _bfScript.Append(" disPoseEvent(); ");
                                
                                for (int i = 0; i < _mapData.Count; i++)
                                {
                                    // Get the marker text for the map data object
                                    _markerText = MapDataCommon.GetMapMarkerText(_mapData[i]);

                                    _bfMarkerScript.Append("if(point !=null){ point =null; }");
                                    _bfMarkerScript.Append("  point=new GPoint(" + _mapData[i].Longitude + "," + _mapData[i].Latitude + ");");
                                    _bfMarkerScript.Append(" if(icon0 !=null ){ icon0 = null;}");
                                    _bfMarkerScript.Append(" icon0 = new GIcon();");
                                    _bfMarkerScript.Append("icon0.image = \\'../Icon/" + _mapData[i].IconName + ".png\\';");
                                    _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
                                    _bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight/2 + "," + iconWidth/2 + ");");
                                    _bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
                                    _bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");


                                    _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");
                                    _bfMarkerScript.Append(" myEvent[" + i + "]= GEvent.addListener(marker[" + i + "],\\'click\\',function(){");
                                    _bfMarkerScript.Append(" ");
                                    _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(\\'" + _markerText.Trim() + "\\'); });");
                                    _bfMarkerScript.Append(" map.addOverlay(marker[" + i + "]);");

                                    _lat = _mapData[i].Latitude;
                                    _lng = _mapData[i].Longitude;
                                }
                               
                                _script.Append("map.clearOverlays();");
                                _script.Append(" zoomEvent= GEvent.addListener(map,\\'zoomend\\',afterZoomEnd);");
                                                                
                                _script.Append("document.getElementById(\\'_Zoom\\').value=map.getZoom().toString();");                                
                                _script.Append("afterZoomEnd();");

                                _script.Append(_bfMarkerScript.ToString());

                                _bfScript.Append("eval(\"" + _script.ToString() + "\"); ");
                                _bfScript.Append("</script>");


                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "_RefreshMainMap", _bfScript.ToString(), false);
                                
                                
                            }
                        }

                        catch (Exception ex)
                        {
                			Console.WriteLine(ex.Message.ToString());
                        }
                        finally
                        {
                            _mapData = null;
                            _processMapData = null;
                        }

                    }
                    catch (Exception ex)
                    { 
                		Console.WriteLine(ex.Message.ToString());
                    }
                    Thread.Sleep(3000);
                }
                   
            }
        }
    }
    protected void _timer_Tick(object sender, EventArgs e)
    {
        
    }

    private string GetMarkerScript()
    {

		if (Request.Params["tID"] == null)
		{
		    try
		    {

		        StringBuilder _bfScript = new StringBuilder();
		        StringBuilder _bfMarkerScript = new StringBuilder();
		        StringBuilder _script = new StringBuilder();


		        IList<MapData> _mapData;
		        ProcessMainMapData _processMapData = new ProcessMainMapData(0);

		        _processMapData.ComID = int.Parse(Session["trkCompany"].ToString());
		        _processMapData.UserID = int.Parse(Session["uID"].ToString());

		        try
		        {
		            _processMapData.invoke();
		            _mapData = _processMapData.MapData;

		            if (_mapData.Count > 0)
		            {

		                string _markerText = "";
		                decimal _lat = 0, _lng = 0;

		                
		                //_bfScript.Append(" disPoseEvent(); ");

		                for (int i = 0; i < _mapData.Count; i++)
		                {
		                    // Get the marker text for the map data object
		                    _markerText = MapDataCommon.GetMapMarkerText(_mapData[i]);

		                    //_bfMarkerScript.Append(" info[" + i + "] = '" + _markerText + "'");
		                    _bfMarkerScript.Append("reDraw = \'false\' ;");
		                    _bfMarkerScript.Append("if(marker[" + i + "] !=null){");
		                    _bfMarkerScript.Append(" marker[" + i + "].setPoint(new GLatLng(" + _mapData[i].Latitude + ", " + _mapData[i].Longitude + "));");
		                    _bfMarkerScript.Append("GEvent.removeListener(myEvent[" + i + "]);");
		                    _bfMarkerScript.Append("myEvent[" + i + "]=null;");
		                    _bfMarkerScript.Append("myEvent[" + i + "]= GEvent.addListener(marker[" + i + "],\'click\',function(){");
		                    _bfMarkerScript.Append(" ");
		                    _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(\'" + _markerText + "\'); });");
		                    _bfMarkerScript.Append("label[" + i + "].setPoint(new GLatLng(" + _mapData[i].Latitude + ", " + _mapData[i].Longitude + "));");
		                    _bfMarkerScript.Append(" marker[" + i + "].redraw(true);");
		                    _bfMarkerScript.Append("reDraw = \'true\' ;");
		                    _bfMarkerScript.Append("}");

		                    _bfMarkerScript.Append("if(reDraw == \'false\'){");
		                    _bfMarkerScript.Append("  point=new GPoint(" + _mapData[i].Longitude + "," + _mapData[i].Latitude + ");");
		                    _bfMarkerScript.Append(" icon0 = new GIcon();");
		                    _bfMarkerScript.Append("icon0.image = \'../Icon/" + _mapData[i].IconName + ".png\';");
		                    _bfMarkerScript.Append(" icon0.iconSize = new GSize(" + iconHeight + "," + iconWidth + ");");
		                    _bfMarkerScript.Append("icon0.iconAnchor = new GPoint(" + iconHeight / 2 + "," + iconWidth / 2 + ");");
		                    _bfMarkerScript.Append("icon0.infoWindowAnchor = new GPoint(15, 15);");
		                    _bfMarkerScript.Append("icon0.infoShadowAnchor = new GPoint(18, 25);");


		                    _bfMarkerScript.Append(" marker[" + i + "]=new GMarker(point,icon0);");
		                    _bfMarkerScript.Append(" myEvent[" + i + "]=GEvent.addListener(marker[" + i + "],\'click\',function(){");
		                    _bfMarkerScript.Append(" ");
		                    _bfMarkerScript.Append(" marker[" + i + "].openInfoWindowHtml(info[" + i + "]); });");
		                    _bfMarkerScript.Append(" map.addOverlay(marker[" + i + "]);");

		                    _bfMarkerScript.Append("}");
		                    

		                    _lat = _mapData[i].Latitude;
		                    _lng = _mapData[i].Longitude;
		                }

		                //_script.Append("map.clearOverlays();");
		               // _script.Append(" zoomEvent= GEvent.addListener(map,\'zoomend\',afterZoomEnd);");

		                //_script.Append("document.getElementById(\'_Zoom\').value=map.getZoom().toString();");
		                //_script.Append("afterZoomEnd();");

		                _script.Append(_bfMarkerScript.ToString());

		                return _script.ToString();
		                return "";

		            }
		        }

		        catch (Exception ex)
		        {
		            Console.WriteLine(ex.Message.ToString());
		        }
		        finally
		        {
		            _mapData = null;
		            _processMapData = null;
		        }

		    }
		    catch (Exception ex)
		    {
		    	Console.WriteLine(ex.Message.ToString());
		    }
		}
    	return "";
    }
}
