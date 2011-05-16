using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessMapData;
using AlarmasABC.DAL.Queries;
using AlarmasABC.DAL.Misc;

/// <summary>
/// This class contains routines for common map activities such
/// as creating map marker text for the unit and bread crumb popups.
/// </summary>
public class MapDataCommon
{
	private const string dateFormat = "MM/dd/yyyy";
    private const string dateTimeFormat = "MM/dd/yyyy hh:mm:ss tt";
    private const string utcDateTimeFormat =  "MM/dd/yyyy HH:mm:ss";
	
	public string DateFormat
	{
		get { return dateFormat; }
	}

	public string DateTimeFormat
	{
		get { return dateTimeFormat; }
	}
	
	
    public MapDataCommon()
    {
        //
        // TODO: Add constructor logic here
        //
    }

	static public void GetAlert(ref AlertData alert, string comID, string deviceID, 
																string recDateTime)
	{
		string strSQL = "SELECT unitID FROM tblUnits WHERE deviceID = " + deviceID + 
						" and comID = " + comID;
		ProcessAlerts processAlerts = new ProcessAlerts();
		ExecuteSQL exec = new ExecuteSQL();
		DataSet ds;
		int unitID;

		try
		{
			ds = exec.getDataSet(strSQL);
			unitID = int.Parse(ds.Tables[0].Rows[0]["unitID"].ToString());
		
			processAlerts.UnitID = unitID;
			processAlerts.ComID = int.Parse(comID);
			processAlerts.AlertTime = recDateTime;
			processAlerts.GetAlert();
			alert = processAlerts.Alert;
		}
		catch (Exception ex)
		{
			Console.WriteLine("GetAlert(): " + ex.Message);
		}
		finally 
		{
			processAlerts = null;
			exec = null;
		}
	}

	static public string GetAlertColor(AlertData alert)
	{
		string color = "green";
		string alertType = alert.AlertType;

		if (alertType == "Speeding" || alertType == "Geofence" || alertType == "Red Alert") 
		{
			color = "red";
		} 
		else if (alertType == "Event" || alertType == "Time") 
		{
			color = "yellow";
		}

		return color;
	}

	static public ReverseGeocoding GetReverseGeocoding(string lat, string lon)
	{
		ReverseGeocoding rg = new ReverseGeocoding(lat, lon);
		try
		{
			rg.GetReverseGeocoding();
			return rg;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}

		return null;
	}

    /// <summary>
    /// Gets formatted marker text for the supplied MapData object.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    static public string GetMapMarkerText(AlarmasABC.Core.Tracking.MapData data)
    {
        string markerText;

        markerText = "<h6>";
		markerText += GetCoreMapMarkerText(data);
        markerText += "</h6>";

		return markerText;
    }

    static public string GetMapMarkerText(AlarmasABC.Core.Tracking.MapData data,
										  string alert)
    {
        string markerText;

        markerText = "<h6>";
		markerText += GetCoreMapMarkerText(data);
		if (alert.Length > 0) {
            markerText += " Alert: " + alert + "<br>";
		}
        markerText += "</h6>";

		return markerText;
    }

	static private string GetCoreMapMarkerText(AlarmasABC.Core.Tracking.MapData data)
	{
        string markerText = "";
        DateTime local, utc;
		ReverseGeocoding rg = GetReverseGeocoding(data.Latitude.ToString(), 
												  data.Longitude.ToString());

		//ClientTimeZone ctz = new ClientTimeZone();
		
        // Convert the time to UTC
        utc = DateTime.Parse(data.RecTimeRevised);
		local = XtremeK.TimeZone.ToLocalTime(utc);

        markerText += " Unit Name: " + data.UnitName + " (" + data.DeviceID + ")" + "<br>";
        markerText += " Local Time: " + local.ToString(dateTimeFormat) + " <br>";
        markerText += " UTC Time: " + utc.ToString(utcDateTimeFormat) + "<br>";

        markerText += " Speed: " + data.Velocity + " mph <br>";

		if (rg != null)
		{
		    if (rg.PostalCode.Length > 0)
		        markerText += " Zip Code: " + rg.PostalCode + "<br>";
		    if (rg.City.Length > 0)
		        markerText += " City: " + rg.City + "<br>";
		    if (rg.County.Length > 0)
		        markerText += " County: " + rg.County + "<br>";
		    if (rg.State.Length > 0)
		        markerText += " State: " + rg.State + "<br>";
		    if (rg.Country.Length > 0)
		        markerText += "Country: " + rg.Country + "<br>";
		}

        return markerText.Replace("'", "");
	}
		

    static public string GetDataSetMarkerText(DataSet ds, int row)
    {
        string markerText = "";

        markerText += "<h6>";
		markerText += GetCoreDataSetMarkerText(ds, row);
        markerText += "</h6>";

        return markerText;
    }

    static public string GetDataSetMarkerText(DataSet ds, int row, string alert)
    {
        string markerText = "";

        markerText += "<h6>";
		markerText += GetCoreDataSetMarkerText(ds, row);
		if (alert.Length > 0) {
            markerText += " Alert: " + alert + "<br>";
		}
        markerText += "</h6>";

        return markerText;
    }
	
	static public string GetReverseGeocodingString(string lat, string lon)
	{
		ReverseGeocoding rg = GetReverseGeocoding(lat, lon);
		
		return rg.City + ", " + rg.State + ", " + rg.Country + ", " + rg.PostalCode;
	}


	static public string GetCoreDataSetMarkerText(DataSet ds, int row)
	{
		string markerText = "";
        DateTime local, utc;
		ReverseGeocoding rg = GetReverseGeocoding(ds.Tables[0].Rows[row]["lat"].ToString(), 
												  ds.Tables[0].Rows[row]["long"].ToString());
		
        utc = DateTime.Parse(ds.Tables[0].Rows[row]["recDateTime"].ToString());
		local = XtremeK.TimeZone.ToLocalTime(utc);

        markerText += " Unit Name: " + ds.Tables[0].Rows[row]["unitName"].ToString() + " (" +
                        ds.Tables[0].Rows[row]["deviceID"].ToString() + ")" + "  <br>";
        markerText += " Local Time: " + local.ToString(dateTimeFormat) + " <br>";
        markerText += " UTC Time: " + utc.ToString(utcDateTimeFormat) + " <br>";
        markerText += " Speed: " + ds.Tables[0].Rows[row]["velocity"].ToString() + " mph <br>";
		if (rg != null) {
		    if (rg.PostalCode.Length > 0)
		        markerText += " Zip Code: " + rg.PostalCode + "<br>";
		    if (rg.City.Length > 0)
		        markerText += " City: " + rg.City + "<br>";
		    if (rg.County.Length > 0)
		        markerText += " County: " + rg.County + "<br>";
		    if (rg.State.Length > 0)
		        markerText += " State: " + rg.State + "<br>";
		    if (rg.Country.Length > 0)
		        markerText += " Country: " + rg.Country + "<br>";
		}

		return markerText.Replace("'", "");
	}
}
