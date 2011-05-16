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
using System.Data.SqlClient;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessSafetyZone;

public partial class Tracking_SafetyZones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string _unitID = "";

            if (Request.QueryString["unitID"] != null)
            {
                _unitID = Request.QueryString["unitID"].ToString();
                LoadZoneList(_unitID);
            }
        }
    }

    private void LoadZoneList(string _unitID)
    {
        DataSet _ds = new DataSet();
        int _comID = int.Parse(Session["trkCompany"].ToString());
        ProcessSafetyZone _zoneList = new ProcessSafetyZone(AlarmasABC.BLL.InvokeOperations.operations.SELECT);

        try
        {
            _zoneList.ComID = _comID;
            _zoneList.UnitID = int.Parse(_unitID);
            _zoneList.invoke();
            _ds = _zoneList.Ds;
            _grdZones.DataSource = _ds;
            _grdZones.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        if (_grdZones.Items.Count < 1)
        {
            _grdZones.Visible = false;
            _lblMessage.Text = "No zones are defined for this unit. Click the \"Create New Zone\" button to add one.";
            _lblMessage.CssClass = "";
            //Response.Redirect("~/Map/SafetyZoneSetup.aspx?unitID="+_unitID+"");
        }
    }

    protected void _lnkNewZone_Click(object sender, EventArgs e)
    {
        
        string _unitID = Request.QueryString["unitID"].ToString();
        Response.Redirect("../Map/SafetyZoneSetup.aspx?unitID=" + _unitID + "");
    }
    protected void _grdZones_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void _grdZone_DataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string isActive = e.Item.Cells[0].Text;
            Label _lblStatus = (Label)e.Item.FindControl("_lblStatus");


            if (isActive == "True")
            {
                _lblStatus.Text = "Zone is ON";
            }
            else if (isActive == "False")
            {
                _lblStatus.Text = "Zone is OFF";
            }
        }
    }
    protected void _grdZones_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        string _unitID = Request.QueryString["unitID"].ToString();
        if (e.CommandName == "Edit")
        {


            Label geofenceID = (Label)e.Item.FindControl("_lblGeofenceID");
            Session["geofenceID"] = geofenceID.Text;
            Response.Redirect("~/Map/SafetyZoneSetup.aspx?unitID=" + _unitID + "");
        }
        else if (e.CommandName == "Delete")
        {
            
            Geofence _gf = new Geofence();
            ProcessSafetyZone _zone = new ProcessSafetyZone(AlarmasABC.BLL.InvokeOperations.operations.DELETE);

            Label geofenceID = (Label)e.Item.FindControl("_lblGeofenceID");
            string _comID = Session["trkCompany"].ToString();
            
            try
            {
                _gf.UnitID = int.Parse(_unitID);
                _gf.Id = int.Parse(geofenceID.Text);
                _gf.ComID = int.Parse(_comID);

                _zone.GeoInfo = _gf;
                _zone.invoke();
                LoadZoneList(_unitID);

                _lblMessage.Text = "Safety Zone Deleted Successfully.";

                if (_grdZones.Items.Count < 1)
                {
                    _lblMessage.Text = "No Safety Zone for this Unit";
                    _grdZones.Visible = false;
                }
            }
            catch(Exception ex)
            {
            
            }
            

        }
    }
}
