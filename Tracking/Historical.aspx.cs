using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessMapData;
using AlarmasABC.DAL.Queries;
using AlarmasABC.Utilities;
using AlarmasABC.DAL.Misc;


public partial class Tracking_Historical : System.Web.UI.Page
{
    static int _serialIndex;
	static int _total;
	
	static string deviceID = "";
	
	const int HistoryPageSize = 25;
	
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            resetHistoricalData();
			_serialIndex = 0;
			_total = 0;
			
			deviceID = Request.QueryString["deviceID"].ToString();
			
   			_historyDate.SelectedDate = XtremeK.TimeZone.LocalTime();
			GetHistoricalData();
        }
            
    }
	
	private void resetHistoricalData()
	{
        Session["historicalData"] = null;
        Session["historicalInfo"] = null;
		Session["historicalPageID"] = 0;
	}
	
    protected int serialNumber()
    {		
        return _serialIndex--;
    }

    protected void _grdHistorical_Paging(object sender,DataGridPageChangedEventArgs e)
    {
        try
        {			
			Session["historicalPageID"] = e.NewPageIndex;
			
			_serialIndex = _total -  e.NewPageIndex * HistoryPageSize;

            _grdRecords.CurrentPageIndex = e.NewPageIndex;
            _grdRecords.DataSource = Session["historicalInfo"];
            _grdRecords.DataBind();
        }
        catch (Exception ex)
        { 
        
        }
    }

    protected void _gridRecords_DataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {           
			AlertData _alert = new AlertData();
	        MapDataCommon.GetAlert(ref _alert, Session["trkCompany"].ToString(),
	                             	deviceID, XtremeK.TimeZone.ToServerTime(e.Item.Cells[4].Text).ToString());
				
			Color item_color = new Color();
			
			if (_alert.AlertType == "Event") {
				 item_color = ColorTranslator.FromHtml("#FFF26A");
			} else if (_alert.AlertType == "Geofence" || _alert.AlertType == "Red Alert" || 
					   _alert.AlertType == "Speeding") {
				item_color = ColorTranslator.FromHtml("#FF766A");
			} else {
				item_color = ColorTranslator.FromHtml("#61C04E");
			}
			
			e.Item.BackColor = item_color;
			
			string html_color = ColorTranslator.ToHtml(item_color);
			
            e.Item.Attributes.Add("id", e.Item.ItemIndex.ToString());
            e.Item.Attributes.Add("onclick", "highlightRow(this)");
				
            e.Item.Attributes.Add("onmouseover", "this.style.cursor='pointer';this.style.backgroundColor = '#DCDCDC';");
            e.Item.Attributes.Add("onmouseout", "this.style.cursor='pointer';this.style.backgroundColor = '" + html_color + "';");        
		}
    }


    private void SearchData(string date)
    {
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        ProcessHistoricalMapData _processHistoricalData = new ProcessHistoricalMapData();

        try
        {
            _processHistoricalData.Date = date;
            _processHistoricalData.DeviceID = int.Parse(deviceID);
            _processHistoricalData.ComID = int.Parse(Session["trkCompany"].ToString());
            _processHistoricalData.invoke();
            ds = _processHistoricalData.Ds;
			
			_total = ds.Tables[0].Rows.Count;
			
			_serialIndex = _total;
			
			
			ds2 = createDataSet(ds);
			if (ds2 != null) 
			{
       			Session["tracking"] = null;
				
	            Session["historicalData"] = ds;
	            Session["historicalInfo"] = ds2;
				
				_grdRecords.CurrentPageIndex = 0;
				
	            BindGrid(ds2);
	                
				_hHasValue.Value = ds.Tables[0].Rows.Count.ToString();
	            ChangeMapPage();
                _lblMessage.Text = "";
                _grdRecords.Visible = true;
			} 
			else 
			{
	            _hHasValue.Value = "";
                _lblMessage.ForeColor = System.Drawing.Color.Red;
                _lblMessage.Text = "No available data on " + date;
                _grdRecords.Visible = false;
			}
        }
        catch (Exception ex)
        {
             _lblMessage.ForeColor = System.Drawing.Color.Red;
             _lblMessage.Text = ex.Message.ToString();
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            ds = null;
            ds2 = null;
            _processHistoricalData = null;
        }
    }

    private void ChangeMapPage()
    {
        string _script = "";

        _script  = "<script language=\"javascript\">";
        _script += "setHistoricalMap();";
        _script += "</script>";

        Page.RegisterClientScriptBlock("Change",_script);
    }
	
	private DataSet createDataSet(DataSet ds)
	{
		if (ds.Tables[0].Rows.Count == 0) 
		{
			return null;
		}
		
		DataTable dt = new DataTable();
		
		dt.Columns.Add("location", typeof(string));
		dt.Columns.Add("velocity", typeof(int));
		dt.Columns.Add("distance", typeof(decimal));
		dt.Columns.Add("recDateTime", typeof(string));
		dt.Columns.Add("alert", typeof(string));

		for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
		{	
			DataRow row = dt.NewRow();	
			
			AlertData _alert = new AlertData();
            MapDataCommon.GetAlert(ref _alert, Session["trkCompany"].ToString(),
                             		ds.Tables[0].Rows[i]["deviceID"].ToString(), 
                             		ds.Tables[0].Rows[i]["recDateTime"].ToString());
								
			if (i > 0) 
			{
				double lat1, lon1;
				double lat2, lon2;
				
				lat1 = double.Parse(ds.Tables[0].Rows[i - 1]["lat"].ToString());
				lon1 = double.Parse(ds.Tables[0].Rows[i - 1]["long"].ToString());
						
				lat2 = double.Parse(ds.Tables[0].Rows[i]["lat"].ToString());
				lon2 = double.Parse(ds.Tables[0].Rows[i]["long"].ToString());
					
				dt.Rows[i - 1]["distance"] = DistanceCalculator.CalcDistance(lat1, lon1, lat2, lon2);   
			}
			
			row["location"] = MapDataCommon.GetReverseGeocodingString(ds.Tables[0].Rows[i]["lat"].ToString(), 
				                  									  ds.Tables[0].Rows[i]["long"].ToString());
			row["velocity"] = ds.Tables[0].Rows[i]["velocity"];
			row["recDateTime"] = XtremeK.TimeZone.ToLocalTime(ds.Tables[0].Rows[i]["recDateTime"].ToString()).ToString();
			if (_alert.AlertMessage.Length > 0) {
				row["alert"] = _alert.AlertMessage;
			} else {
				row["alert"] = "Position";
			}
			
			dt.Rows.Add(row);
		}
		
		
		dt.Rows[dt.Rows.Count - 1]["distance"] = 0;
		
		DataSet data = new DataSet();
		data.Tables.Add(dt);
		
		return data;
	}

    private void BindGrid(DataSet ds)
    {
        try
        {
            string date = _historyDate.SelectedDate.ToString();

            _grdRecords.DataSource = ds;
            _grdRecords.DataBind();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            ds = null;
        }
    }

    protected void _btn_Click(object sender, EventArgs e)
    {
        GetHistoricalData();
    }
	
	protected void _historyDate_SelectedDateChanged(object s, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
	{
        GetHistoricalData();
	}

    private void GetHistoricalData()
    {
        try
        {
        	resetHistoricalData();
            SearchData(_historyDate.SelectedDate.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

	}
    
}
