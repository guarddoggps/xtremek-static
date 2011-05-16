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


public partial class Map_V3Map : System.Web.UI.Page,ICallbackEventHandler
{
    int iconHeight = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconHeight"]);
    int iconWidth = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconWidth"]);

	string _script;

    protected void Page_Load(object sender, EventArgs e)
    {
		if (!IsPostBack)
		{
            ClientScriptManager m = Page.ClientScript;
            string str = m.GetCallbackEventReference(this, "args", "ReceiveServerData", "'this is context from server'");
            string strCallback = "function CallServer(args,context){" + str + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", strCallback, true);

			string script = "<script language=\"javascript\">";
			script += GetMarkerScript();
			script += "</script>";
            Page.RegisterClientScriptBlock("marker", script);
		}
    }

	public string GetCallbackResult()
    {
        return _script;
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        
    }

	private IList<MapData> LoadMapData(int comID, int userID, int typeID)
	{
		IList<MapData> mapData = new List<MapData>();
        ProcessMainMapData processMapData = new ProcessMainMapData(typeID != -1 ? 1 : 0);
        processMapData.ComID = comID;
        processMapData.UserID = userID;
		if (typeID != -1) 
		{
			processMapData.TypeID = typeID;
		}

		try
		{
        	processMapData.invoke();
			mapData = processMapData.MapData;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Map_V3Map::LoadMapData(): " + ex.Message);
		}
		finally
		{
			processMapData = null;
		}

		return mapData;
	}

	private string GetMarkerScript()
	{
		StringBuilder script = new StringBuilder();
		StringBuilder markerScript = new StringBuilder();
		IList<MapData> mapData;

		try
		{
			if (Request.Params["tID"] == null)
			{
				mapData = LoadMapData(int.Parse(Session["trkCompany"].ToString()),
									  int.Parse(Session["uID"].ToString()), -1);
			}
			else
			{
				mapData = LoadMapData(int.Parse(Session["trkCompany"].ToString()),
									  int.Parse(Session["uID"].ToString()), 
									  int.Parse(Session["tID"].ToString()));
			}

			if (mapData.Count > 0)
			{
				//string markerText = "";
				for (int i = 0; i < mapData.Count; i++)
				{
					//markerText = MapDataCommon.GetMapMarkerText(mapData[i]);

					markerScript.Append(" var point = new google.maps.LatLng(" + mapData[i].Latitude + ", " + mapData[i].Longitude + ");");
					markerScript.Append(" var icon = new google.maps.MarkerImage('/Icon/" + mapData[i].IconName + ".png',");
					markerScript.Append(" new google.maps.Size(" + iconHeight + "," + iconWidth + "),");
					markerScript.Append(" new google.maps.Point(0, 0),");
					markerScript.Append(" new google.maps.Point(" + iconHeight / 2 + "," + iconWidth / 2 + "));");
					markerScript.Append(" marker[" + i + "] = new google.maps.Marker({ position: point, map: map, icon: icon });");
					Console.WriteLine(markerScript);
				}

				script.Append("eval(\"" + markerScript.ToString() + "\");");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Map_V3Map::GetMarkerScript(): " + ex.Message);
		}
		finally
		{
			markerScript = null;
		}

		Console.WriteLine(script);
		return script.ToString();
	}
}
