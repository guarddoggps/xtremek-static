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
using AlarmasABC.BLL.ProcessCompany;
using AlarmasABC.BLL.ProcessUnitManagement;
using AlarmasABC.BLL.ProcessUnitType;
using AlarmasABC.Core.Admin;

public partial class CompanyAdmin_UnitManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Session["insert"] = true;
            Session["edit"] = true;
			Session["fullAccess"] = true;

            try
            {
                DataSet _ds = new DataSet();
                _ds = FormPermission.LoadPermission();

                if (_ds.Tables[0] != null)
                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in _ds.Tables[0].Rows)
                        {
                            if (row["FormName"].ToString() == "Unit Management")
                            {
                                if (int.Parse(row["insert"].ToString()) == 0)
                                {
            						Session["insert"] = false;
                                }

                                if (int.Parse(row["edit"].ToString()) == 0)
                                {
            						Session["edit"] = false;
									btnSubmit.Visible = false;
                                }

								if (int.Parse(row["fullAccess"].ToString()) == 0)
								{
									cmbVehicleCat.Enabled = false;
									cmbPattern.Enabled = false;
									lstNBT.Visible = false;
									lstBT.Visible = false;
									lblNBT.Visible = false;
									lblBT.Visible = false;
									_btnRight.Visible = false;
									_btnLeft.Visible = false;
									groupHeader.Visible = false;
            						Session["fullAccess"] = false;
								}
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
            }

            LoadList(Session["trkCompany"].ToString());
            LoadImage();
            loadUnit(Session["trkCompany"].ToString());
            loadUnitCat(Session["trkCompany"].ToString());
            loadPattern(Session["trkCompany"].ToString());
            lblMessage.Text = "";
            //lblUnitName.Visible = true;
            cmbUnitName.Visible = true;
            //lblUnitID.Visible = false;
            txtDevice.Enabled = true;
			if (!(bool)Session["insert"]) 
			{
                txtDevice.Enabled = false;
                loadVehicleInfo(cmbUnitName.SelectedValue.ToString(), Session["trkCompany"].ToString());
			}
			else
			{
            	btnSubmit.Text = "Save";
			}
		
        }
    }

    private void LoadList(string comID)
    {
        try
        {
            UserGroup _usrGrp = new UserGroup();
            _usrGrp.ComID = int.Parse(comID);
            ProcessUnitManagementQueries.fillGroupListListBox(lstNBT, _usrGrp);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    private void LoadImage()
    {
        try
        {
            ProcessUnitManagementQueries.LoadIconOnDlist(dlstIcon);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    protected void cmbUnitName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbUnitName.SelectedValue == "0")
            {
				if ((bool)Session["edit"])
				{
                	btnSubmit.Visible = true;
				}
                btnSubmit.Text = "Save";
                txtDevice.Enabled = true;
                clearControls();
                clearComboItems();
                LoadList(Session["trkCompany"].ToString());
                LoadImage();
                loadUnit(Session["trkCompany"].ToString());
                loadUnitCat(Session["trkCompany"].ToString());
                loadPattern(Session["trkCompany"].ToString());
                
                lblMessage.Text = "";
            }
            else
            {
				if ((bool)Session["edit"])
				{
					btnSubmit.Visible = true;
				}
				else
				{
					btnSubmit.Visible = false;
				}
                btnSubmit.Text = "Update";
                txtDevice.Enabled = false;
                loadUnitCat(Session["trkCompany"].ToString());
                //loadModel(Session["trkCompany"].ToString());
                loadPattern(Session["trkCompany"].ToString());
                //LoadFuel();
                loadVehicleInfo(cmbUnitName.SelectedValue.ToString(), Session["trkCompany"].ToString());
                NotListedGroup(cmbUnitName.SelectedValue.ToString(), Session["trkCompany"].ToString());
                ListedGroup(cmbUnitName.SelectedValue.ToString(), Session["trkCompany"].ToString());
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw new Exception (ex.Message.ToString());
        }
    }

    private void NotListedGroup(string UnitID, string ComID)
    {
		lstNBT.Items.Clear();
		UserGroup _usrgrp = new UserGroup();  
		_usrgrp.UnitID = int.Parse(UnitID);
		_usrgrp.ComID = int.Parse(ComID);        
		ProcessUnitManagementQueries.fillNotListedGroupList(lstNBT, _usrgrp);
    }
    private void ListedGroup(string UnitID, string ComID)
    {
     	lstBT.Items.Clear();
      	UserGroup _usrgrp = new UserGroup();
      	_usrgrp.UnitID = int.Parse(UnitID);
     	_usrgrp.ComID = int.Parse(ComID);
     	ProcessUnitManagementQueries.fillListedGroupList(lstBT, _usrgrp);
    }

    private void loadVehicleInfo(string _UnitId, string _ComID)
    {
        Units _units = new Units();
        try
        {
            _units.UnitID = int.Parse(_UnitId);
            ProcessUnitManagementQueries _UnitManagement = new ProcessUnitManagementQueries(AlarmasABC.BLL.InvokeOperations.operations.SELECT);
            _UnitManagement.Units = _units;
            _UnitManagement.invoke();

            DataSet _ds = _UnitManagement.Ds;

            cmbVehicleCat.SelectedValue = _ds.Tables[0].Rows[0]["typeID"].ToString();
            txtLicense.Text = _ds.Tables[0].Rows[0]["LicenseID"].ToString();
            txtName.Text = _ds.Tables[0].Rows[0]["unitName"].ToString();
            txtDevice.Text = _ds.Tables[0].Rows[0]["deviceID"].ToString();
            txtOtherInfo.Text = _ds.Tables[0].Rows[0]["otherInfo"].ToString();
            loadImage(_UnitId, _ComID);
            if (_ds.Tables[0].Rows[0]["patternID"].ToString() != "")
                cmbPattern.SelectedValue = _ds.Tables[0].Rows[0]["patternID"].ToString();
				
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _units = null;
        }
    }

    private void loadImage(string _Unitid, string _comId)
    {
        Units _unit = new Units();
        _unit.UnitID = int.Parse(_Unitid);
        _unit.ComID = int.Parse(_comId);
        ProcessUnitManagementQueries _UnitImage = new ProcessUnitManagementQueries(_unit);
        _UnitImage.loadImage();
        DataSet _ds = _UnitImage.Ds;

        if (_ds.Tables[0].Rows.Count > 0)
        {
            imgIcon.Src = "";
            imgIcon.Src = "../Icon/" + _ds.Tables[0].Rows[0][0].ToString() + ".png";
        }
        else
        {

        }
    }

    private void loadUnit(string ComID)
    {   
        try
        {
            Units _units = new Units();
            _units.ComID = int.Parse(ComID);
			if (!(bool)Session["fullAccess"])
			{
				ProcessUnitManagementQueries.fillDropDownItemsUserUnits(cmbUnitName, _units, Session["uID"].ToString());
				return;
			}

			if ((bool)Session["insert"]) 
			{
            	ProcessUnitManagementQueries.fillDropDownItemsUnits1(cmbUnitName, _units);
			}
			else
			{
            	ProcessUnitManagementQueries.fillDropDownItemsUnits(cmbUnitName, _units);
			}
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    private void loadUnitCat(string ComID)
    {
        AlarmasABC.Core.Admin.UnitType _uType = new AlarmasABC.Core.Admin.UnitType();
        _uType.ComID = int.Parse(ComID);
        ProcessUnitTypeQueries.fillDropDownItems(cmbVehicleCat, _uType);

    }

    private void loadPattern(string ComID)
    {
        Pattern _pattern = new Pattern();
        _pattern.ComID = int.Parse(ComID);
        ProcessUnitManagementQueries.fillDropDownItemsPattern(cmbPattern, _pattern);
    }

    private string formatIcon(string imgUrl)
    {
        int st = imgUrl.LastIndexOf("/");
        int lst = imgUrl.LastIndexOf(".");
        string iconName = imgUrl.Substring(st + 1, (lst - st) - 1);
        return iconName;
    }

    private bool Validated(string iconName)
    {
        if (cmbUnitName.SelectedIndex < 1)
        {
            lblMessage.Text = "Please Select Device ID";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
        else if (cmbVehicleCat.SelectedIndex <= 0)
        {
            if (!findGeneralCat())
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please Select Unit's Category..";
                return false;
            }
        }
        else if (iconName == "")
        {
            lblMessage.Text = "Please Select Unit Icon";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        return true;

    }

    private bool Valid(string iconName)
    {
        //if (cmbUnitModel.SelectedIndex < 0)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    lblMessage.Text = "Please Select Unit Model..";
        //    return false;
        //}
        //else if (cmbFuel.SelectedIndex < 0)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.Red;
        //    lblMessage.Text = "Please Select Unit's Fuel Type..";
        //    return false;
        //}
        if (cmbVehicleCat.SelectedIndex <= 0)
        {
            if (!findGeneralCat())
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please Select Unit's Category..";
                return false;
            }
        }

        else if (txtName.Text == "")
        {
            lblMessage.Text = "Please Enter Unit Name..";
            lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        else if (iconName == "")
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Please Select Unit Icon";
            return false;
        }

        lblMessage.ForeColor = System.Drawing.Color.Green;
        return true;

    }

    private bool findGeneralCat()
    {
        foreach (ListItem item in cmbVehicleCat.Items)
        {
            if (item.Text == "General")
            {
                cmbVehicleCat.SelectedValue = item.Value;
                return true;
            }
        }
        return false;
    }
    private bool CheckInfo(string comID)
    {
        Units _unit = new Units();
        _unit.ComID = int.Parse(comID);
        _unit.UnitID = int.Parse(txtDevice.Text.Trim());
        _unit.UnitName = txtName.Text;
        ProcessUnitManagementQueries processmgmt = new ProcessUnitManagementQueries(_unit);
        processmgmt.Units = _unit;
        processmgmt.checkUnitExist();
        DataSet _ds = processmgmt.Ds;
        if (_ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void clearData(string ID)
    {
        Units _unit = new Units();
        _unit.UnitID = int.Parse(ID);
        ProcessUnitManagementQueries _UmgtQry = new ProcessUnitManagementQueries(_unit);
        _UmgtQry.Units = _unit;
        _UmgtQry.clearUnitData();
    }

    private void clearComboItems()
    {
        cmbPattern.Items.Clear();
        if (cmbUnitName.Items.Count > 0)
            cmbUnitName.SelectedIndex = 0;
        cmbVehicleCat.Items.Clear();
    }
    private void clearControls()
    {

        txtLicense.Text = "";
        //txtDriver.Text = "";
        //txtVIN.Text = "";
        //txtUnitColor.Text = "";
        //txtKeyCode.Text = "";
        //txtDevicePerchaseLocation.Text = "";
        //txtunitCost.Text = "";
        //txtCounterIED.Text = "";
        txtName.Text = "";
        txtDevice.Text = "";
        txtOtherInfo.Text = "";
        //txtWindowtIntScheme.Text = "";
        imgIcon.Src = "";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (btnSubmit.Text == "Save")
        {
            string iconName = "";
            string imgUrl = imgSource.Value;
            if (imgUrl == "")
            {
                if (imgIcon.Src != "")
                {
                    imgUrl = imgIcon.Src;
                    iconName = formatIcon(imgUrl);

                }
                else
                    imgUrl = "";
            }
            else
            {
                iconName = formatIcon(imgUrl);
            }

            if (CheckInfo(Session["trkCompany"].ToString()))
            {
                lblMessage.Text = "This device is already exists for the company";
            }
            else
            {
                if (Valid(iconName))
                    saveInfo(iconName, Session["trkCompany"].ToString());
                if (IsPostBack)
                    loadDeviceID(Session["trkCompany"].ToString());
                clearControls();
                clearComboItems();

            }
        }

        else if (btnSubmit.Text == "Update")
        {


            string icnName = "";
            string imageUrl = imgSource.Value;
            if (imageUrl == "")
            {
                if (imgIcon.Src != "")
                {
                    imageUrl = imgIcon.Src;
                    icnName = formatIcon(imageUrl);

                }
                else
                    imageUrl = "";
            }
            else
            {
                icnName = formatIcon(imageUrl);
            }
            if (Validated(icnName))
                updateInfo(icnName, Session["trkCompany"].ToString());
            clearControls();
            clearComboItems();

        }


    }

    private void loadDeviceID(string comID)
    {
        Units _units = new Units();
        _units.ComID = int.Parse(comID);
        ProcessUnitManagementQueries.fillDropDownItemsDeviceID(cmbUnitName, _units);
    }


    private void saveInfo(string iconName, string companyID)
    {

        string patternID = cmbPattern.SelectedValue.ToString();
        int comID = int.Parse(companyID);

        Units _units = new Units();
        ProcessUnitManagementQueries _unitManagement = new ProcessUnitManagementQueries(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
        ProcessUnitManagementQueries _umg = new ProcessUnitManagementQueries();
        DataSet _ds = new DataSet();
        try
        {
            _units.UnitID = int.Parse(txtDevice.Text.Trim());
            _units.UnitName = txtName.Text.Trim();
            _units.LicenseID = txtLicense.Text.Trim();
            _units.TypeID = int.Parse(cmbVehicleCat.SelectedValue.ToString());
            _units.IconName = iconName;
            _units.ComID = comID;
            _units.OtherInfo = txtOtherInfo.Text.Trim();
            _units.PatternID = int.Parse(patternID);

            _unitManagement.Units = _units;
            _unitManagement.invoke();

            _umg.MaxUnitId();
            _ds = _umg.Ds;
            int UnitID = int.Parse(_ds.Tables[0].Rows[0][0].ToString());


            saveUserInfo(UnitID, companyID);

            lblMessage.Text = "Unit \"" + _units.UnitName + "\" Setup Successfully...";

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _units = null;
            _unitManagement = null;
            _umg = null;
            _ds = null;
        }




    }

    private bool saveUserInfo(int UnitID, string companyID)
    {
        string _listValue = _hListValue.Value;
        string[] _listItem = _listValue.Split(';');

        for (int i = 0; i < _listItem.Length - 1; ++i)
        {

            Units _unit = new Units();
            ProcessUnitManagementQueries _unitmangmnt = new ProcessUnitManagementQueries(_unit);
            try
            {
                _unit.GroupID = int.Parse(_listItem[i]);
                _unit.UnitID = UnitID;
                _unit.ComID = int.Parse(companyID);

                _unitmangmnt.Units = _unit;
                _unitmangmnt.addUnitUserInfo();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                _unit = null;
                _unitmangmnt = null;

            }
        }

        return true;

    }


    private void updateInfo(string iconName, string companyID)
    {
        string patternID = cmbPattern.SelectedValue.ToString();
        int comID = int.Parse(companyID);

        //DateTime pDate = new DateTime();
        //DateTime dDate = new DateTime();
        //if (txtUnitPerchaseDate.SelectedDate.ToString() != "")
        //    pDate = Convert.ToDateTime(txtUnitPerchaseDate.SelectedDate.ToString());
        //else
        //    pDate = Convert.ToDateTime("01/01/2000");
        //if (txtDevicePerchaseDate.SelectedDate.ToString() != "")
        //    dDate = Convert.ToDateTime(txtDevicePerchaseDate.SelectedDate.ToString());
        //else
        //    dDate = Convert.ToDateTime("01/01/2000");

        Units _units = new Units();
        ProcessUnitManagementQueries _unitManagement = new ProcessUnitManagementQueries(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
        try
        {
            _units.UnitID = int.Parse(cmbUnitName.SelectedValue.ToString());
            _units.UnitName = txtName.Text.Trim();
            _units.LicenseID = txtLicense.Text.Trim();
            //_units.DriverName = txtDriver.Text.Trim();
            //_units.VIN = txtVIN.Text.Trim();
            //_units.ModelID = int.Parse(cmbUnitModel.SelectedValue.ToString());
            //_units.Unitpurchasedate = pDate;
            //_units.LevelArmor = cmbArmor.SelectedValue.ToString();
            //_units.Wtint = txtWindowtIntScheme.Text.Trim();
            //_units.Package = cmbCommonPackage.SelectedValue.ToString();
            //_units.UnitColor = txtUnitColor.Text.Trim();
            //_units.KeyCode = txtKeyCode.Text.Trim();
            //_units.Unitfueltype = int.Parse(cmbFuel.SelectedValue.ToString());
            //_units.DevicePurchaseLocation = txtDevicePerchaseLocation.Text.Trim();
            //_units.DevicePurchaseDate = dDate;
            //if (txtunitCost.Text.ToString() != "")
            //{
            //    _units.Unitcost = int.Parse(txtunitCost.Text.Trim());
            //}
            //else
            //{
            //    txtunitCost.Text = "0";
            //    _units.Unitcost = int.Parse(txtunitCost.Text.Trim());
            //}
            //_units.CounterIED = txtCounterIED.Text.Trim();
            _units.TypeID = int.Parse(cmbVehicleCat.SelectedValue.ToString());
            _units.IconName = iconName;
            _units.OtherInfo = txtOtherInfo.Text.Trim();
            _units.PatternID = int.Parse(patternID);


            _unitManagement.Units = _units;
            _unitManagement.invoke();

            int UnitID = int.Parse(cmbUnitName.SelectedValue.ToString());

            if (updateUserInfo(UnitID, companyID))
            {
                lblMessage.Text = "Unit " + _units.UnitName + " Updated Successfully...";
                clearControls();

            }
            else
                clearData(UnitID.ToString());


        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _units = null;
            _unitManagement = null;
        }
    }


    private bool updateUserInfo(int UnitID, string companyID)
    {
        string _listValue = _hListValue.Value;
        string[] _listItem = _listValue.Split(';');

        if (_listItem.Length > 1)
        {
            deleteUnitUserInforamtion(UnitID, companyID);

            for (int i = 0; i < _listItem.Length - 1; ++i)
            {

                Units _unit = new Units();
                ProcessUnitManagementQueries _unitmangmnt = new ProcessUnitManagementQueries(_unit);
                try
                {
                    _unit.GroupID = int.Parse(_listItem[i]);
                    _unit.UnitID = UnitID;
                    _unit.ComID = int.Parse(companyID);

                    _unitmangmnt.Units = _unit;
                    _unitmangmnt.updateUnitUserInfo();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
                finally
                {
                    _unit = null;
                    _unitmangmnt = null;

                }
            }

           
        }
       return true;
    }
    private void deleteUnitUserInforamtion(int UnitId, string ComID)
    {
        Units _unit = new Units();
        ProcessUnitManagementQueries _unitmangmnt = new ProcessUnitManagementQueries(_unit);
        try
        {
            _unit.UnitID = UnitId;
            _unit.ComID = int.Parse(ComID);

            _unitmangmnt.Units = _unit;
            _unitmangmnt.deleteUnitUserInfo();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _unit = null;
            _unitmangmnt = null;

        }
    }

    private void Create()
    {
	    //lblUnitName.Visible = false;
            cmbUnitName.Visible = false;
            lblUnitID.Visible = true;
            txtDevice.Visible = true;
            clearComboItems();
            clearControls();
            LoadList(Session["trkCompany"].ToString());
            loadUnitCat(Session["trkCompany"].ToString());
            //loadModel(Session["trkCompany"].ToString());
            loadPattern(Session["trkCompany"].ToString());
            //LoadFuel();


            lblMessage.Text = "";
            //btnCreateUnit.Text = "Work With Existing Unit";
            btnSubmit.Text = "Save";
    }

    public void Update()
    {
        //lblUnitName.Visible = true;
        cmbUnitName.Visible = true;
        lblUnitID.Visible = false;
        txtDevice.Visible = false;
        clearComboItems();
        clearControls();
        loadUnit(Session["trkCompany"].ToString());
        lblMessage.Text = "";
        //btnCreateUnit.Text = "Create New Unit";
        btnSubmit.Text = "Update";
    }

}
