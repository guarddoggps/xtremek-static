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
using AlarmasABC.BLL.ProcessUnitType;
using AlarmasABC.BLL.ProcessUnit;
using AlarmasABC.Core.Tracking;
using AlarmasABC.BLL.ProcessSafetyZone;
using System.Text;
using AlarmasABC.DAL.Queries;


public partial class Map_Geofence : System.Web.UI.Page
{
    string _str;
    int iconHeight = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconHeight"]);
    int iconWidth = System.Convert.ToInt32(ConfigurationManager.AppSettings["iconWidth"]);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ClientScriptManager m = Page.ClientScript;
            //string str = m.GetCallbackEventReference(this, "args", "ReceiveServerData", "'this is context from server'");
            //string strCallback = "function CallServer(args,context){" + str + ";}";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", strCallback, true);

            string _key = ConfigurationManager.AppSettings["gKey"].ToString();
            string _mapKey = "<script src='http://maps.google.com/maps?file=api&v=2&key=" + _key + "' type='text/javascript'></script>";
            Page.RegisterClientScriptBlock("", _mapKey);


           
            int _comID = int.Parse(Session["trkCompany"].ToString());
            loadUnitType(_comID);
        }
    }

    private void loadUnitType(int _comID)
    {
        AlarmasABC.Core.Admin.UnitType _unitType = new AlarmasABC.Core.Admin.UnitType();

        try
        {
            _unitType.ComID = _comID;
            ProcessUnitTypeQueries.fillListBoxItems(_lstNBT, _unitType);

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    private void LoadUnits(int _comID)
    {
        try
        {
            ProcessUnitQueries.fillListBox(_lstNBT, _comID);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }



    private bool validInput(decimal _radius)
    {
        if (_hListValue.Value.Length < 1)
        {
            _lblMessage.Text = "Please select any unit group or unit";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (_txtZoneName.Text == "")
        {
            _lblMessage.Text = "Please Enter Geofence name";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (_radius == 0)
        {
            _lblMessage.Text = "Radius can't be 0";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (_txtEmail.Text == "")
        {
            _lblMessage.Text = "Invalid Email";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        return true;
    }
    private void clearControls()
    {
        _txtZoneName.Text = "";
        _txtEmail.Text = "";
    }

    protected void _btnClear_Click(object sender, EventArgs e)
    {

    }
    protected void _btnOK_Click(object sender, EventArgs e)
    {
        double _lat = 0;
        double _lng = 0;
        decimal _radius = 0;

        try
        {
            _lat = double.Parse(_Lat.Value);
            _lng = double.Parse(_Long.Value);
            _radius = decimal.Parse(_Radius.Value);

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

       

       if (validInput(_radius))
        {
             saveGeofence(_lat, _lng, _radius);
            _lblMessage.ForeColor = System.Drawing.Color.Green;
            _lblMessage.Text = " Geofence " + _txtZoneName.Text.Trim() + " Created.";
            clearControls();            
        }
    }
    protected void _btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void _rdUnitGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdUnitGroup.Checked)
        {
            int _comID = int.Parse(Session["trkCompany"].ToString());
            loadUnitType(_comID);
            UpdatePanelNList.Update();
            addList.Update();
        }
    }
    protected void _rdUnits_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdUnits.Checked)
        {
            int _comID = int.Parse(Session["trkCompany"].ToString());
            LoadUnits(_comID);
            UpdatePanelNList.Update();
            addList.Update();
        }
    }

    private void saveGeofence(double _lat, double _lng, decimal _radius)
    {
        try
        {
            
            string _comID = Session["trkCompany"].ToString();
            string _listValue = _hListValue.Value;
            string[] _listItem = _listValue.Split(';');
            bool isActive = _rdoON.Checked;
            if (_rdUnits.Checked)
            {

                for (int i = 0; i < _listItem.Length - 1; i++)
                {
                    ProcessSafetyZone _unitInsert = new ProcessSafetyZone();
                    Geofence _geoObj = new Geofence();

                    _geoObj.ComID = int.Parse(_comID);
                    _geoObj.UnitID = int.Parse(_listItem[i].ToString());
                    _geoObj.Name = _txtZoneName.Text.ToString();
                    _geoObj.Email =_txtEmail.Text.ToString() ;
                    _geoObj.CenterLat = decimal.Parse(_lat.ToString()) ;
                    _geoObj.CenterLong =decimal.Parse(_lng.ToString() );
                    _geoObj.Radius = _radius ;
                    _geoObj.IsActive = isActive;

                    _unitInsert.GeoInfo = _geoObj;
                    _unitInsert.GeofenceUnitInsert();

                    //_lblMessage.Text = "Sucessfull";

                }

            }

            else if (_rdUnitGroup.Checked)
            {
                for (int i = 0; i < _listItem.Length - 1; i++)
                {
                    ProcessSafetyZone _unitInsert = new ProcessSafetyZone();
                    Geofence _geoObj = new Geofence();

                    _geoObj.ComID = int.Parse(_comID);
                    _geoObj.UnitGroupID = int.Parse(_listItem[i].ToString());
                    _geoObj.Name = _txtZoneName.Text.ToString();
                    _geoObj.Email = _txtEmail.Text.ToString();
                    _geoObj.CenterLat = decimal.Parse(_lat.ToString());
                    _geoObj.CenterLong = decimal.Parse(_lng.ToString());
                    _geoObj.Radius = _radius;
                    _geoObj.IsActive = isActive;

                    _unitInsert.GeoInfo = _geoObj;
                    _unitInsert.GeofenceUnitGroupInsert();
                }
            }
        }

        catch (Exception ex)
        {
            throw new Exception (" XtremeK::Map::Geofence " + ex.Message);
        }
    }

    //public string GetCallbackResult()
    //{
    //    return _str.ToString();

    //}

    //public void RaiseCallbackEvent(string eventArgument)
    //{
    //    _str = GetMarkerText();
    //}

    
}
