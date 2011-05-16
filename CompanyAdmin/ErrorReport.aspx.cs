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
using AlarmasABC.BLL.ErrorReports;
using AlarmasABC.Core.Admin;




public partial class Widgets_ErrorReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //setInitialDate();
        }
    }

    private void setInitialDate()
    {
        string _strScript = "<script language=\"javascript\">";
        _strScript += " setTimeout('get2Day()',500);";
        _strScript += "</script>";

        Page.RegisterClientScriptBlock("setDate",_strScript);
    }
    private void loadGridData()
    {
        try
        {
            string _startTime = _dtStart.SelectedDate.ToString();
            string _endTime=_dtEnd.SelectedDate.ToString();

            string _serviceName = "";
            if (_rdSpeedingAlarm.Checked)
                _serviceName = "Speeding";
            else if (_rdGeofence.Checked)
                _serviceName = "Geofence";
            else if (_rdGeocoding.Checked)
                _serviceName = "Geocoding";
           
            #region old_1
            //string _strSQL = "select * from tblErrorLog where serviceName='" + _serviceName + "' and errorTime between '" + _startTime + "' and '" + _endTime + "'  order by errorTime desc";
            //Database _db = new Database();
            DataSet _ds = new DataSet();

            //_ds = _db.getDataSet(_strSQL);
            #endregion
            ErrorReport _errorReport = new ErrorReport();
            _errorReport.ServiceName = _serviceName;
            _errorReport.StartTime = _startTime;
            _errorReport.EndTime = _endTime;

            ProcessErrorReport _errorReports = new ProcessErrorReport();
            _errorReports.ErrorReport = _errorReport;
            _errorReports.invoke();
            _ds = _errorReports.Ds;

            #region new_1

            #endregion

            int count = _ds.Tables[0].Rows.Count;

            _grdErrorLog.DataSource = _ds;
            _grdErrorLog.DataBind();

            if (_grdErrorLog.Items.Count < 1)
                _grdErrorLog.Visible = false;
            else
                _grdErrorLog.Visible = true;
        }
        catch (Exception ex)
        { 
        
        }

    }
    protected void _rdSpeedingAlarm_CheckedChanged(object sender, EventArgs e)
    {
       // loadGridData();
    }
    protected void _rdGeofence_CheckedChanged(object sender, EventArgs e)
    {
       // loadGridData();
    }
    protected void _rdGeocoding_CheckedChanged(object sender, EventArgs e)
    {
       // loadGridData();
    }
    protected void _btnShow_Click(object sender, EventArgs e)
    {
        loadGridData();
    }
    protected void _btn2Day_Click(object sender, EventArgs e)
    {
        loadGridData();
    }
    protected void _btnYesterDay_Click(object sender, EventArgs e)
    {
        loadGridData();
    }
    protected void _btnLastWeek_Click(object sender, EventArgs e)
    {
        loadGridData();
    }
    protected void _btnLastMonth_Click(object sender, EventArgs e)
    {
        loadGridData();
    }
    
}
