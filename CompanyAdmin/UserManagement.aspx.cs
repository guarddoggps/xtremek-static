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
using AlarmasABC.BLL.ProcessUser;
using AlarmasABC.BLL.ProcessCompany;
using AlarmasABC.BLL.ProcessUserGroup;
using AlarmasABC.BLL.ProcessUnitType;
using AlarmasABC.BLL.ProcessUnit;
using AlarmasABC.BLL.ProcessUserWiseUnitCat;
using AlarmasABC.Core.Admin;
using AlarmasABC.DAL.Queries;
using AlarmasABC.BLL.ProcessUnitUserWise;
using AlarmasABC.BLL.ProcessSecurityScheme;
using AlarmasABC.Utilities;

public partial class CompanyAdmin_UserManagement : System.Web.UI.Page
{
    private int uID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
			bool insert = true;
            Session["insert"] = true;
            Session["edit"] = true;
            Session["fullAccess"] = true;
            try
            {
                DataSet _ds = new DataSet();
                _ds = FormPermission.LoadPermission();
                if (_ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in _ds.Tables[0].Rows)
                    {
                        if (row["FormName"].ToString() == "User Management")
                        {
                            if (int.Parse(row["insert"].ToString()) == 0)
                            {
                                Session["insert"] = false;
								insert = false;
								_btnSave.Text = "Update";
                            }

                            if (int.Parse(row["edit"].ToString()) == 0)
                            {
                                Session["edit"] = false;
                            	_btnSave.Visible = false;
                                _btnCancel.Text = "Close";
                            }

							if (int.Parse(row["fullAccess"].ToString()) == 0)
							{
            					Session["fullAccess"] = false;
								_ddlUser.Enabled = false;
								_txtLoginName.Enabled = false;
								_ddlGroup.Enabled = false;
								_ddlSecurityScheme.Enabled = false;
								_chkIsActive.Enabled = false;
								unitGroupContainer.Visible = false;
							}
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
            	Console.WriteLine(ex.Message.ToString());
            }

            _divUser.Visible = true;
            int _comID = int.Parse(Session["trkCompany"].ToString());
            loadUser(_comID, insert);
            loadGroup(_comID);
            loadUnitType(_comID);
            loadSecurityQuestion();
            loadSecurityScheme();
            _chkIsActive.Checked = true;
            _ddlSecurityScheme.SelectedIndex = 0;
            string uID = Session["uID"].ToString();
            _ddlUser.SelectedValue = uID;
            _btnSave.Text = "Update";

            loadUserInfo(int.Parse(_ddlUser.SelectedValue));
            if (_rdoUnitGroup.Checked)
            {
                _loadUnderRdoUnitGroup();
            }
            if (_rdoUnits.Checked)
            {
                _loadUnderRdoUnit();
            }
            
        }
    }

    private void loadSecurityScheme()
    {
        try
        {
            DataSet _ds = new DataSet();

            int _comID = int.Parse(Session["trkCompany"].ToString());
            ProcessScheme.fillDropDownItems(_ddlSecurityScheme,_comID);
            
            string _strSQL = "SELECT id FROM tblSecurityScheme WHERE comID = " 
                              + _comID + " AND defaultScheme = '1';";

            ExecuteSQL _sql = new ExecuteSQL();

            _ds = _sql.getDataSet(_strSQL);
            if (_ds.Tables[0].Rows.Count > 0)
                _ddlSecurityScheme.SelectedValue = _ds.Tables[0].Rows[0]["ID"].ToString();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    private void loadUnitType(int _comID)
    {
        AlarmasABC.Core.Admin.UnitType _unitType = new AlarmasABC.Core.Admin.UnitType();

        try
        {
            _unitType.ComID = _comID;
            ProcessUnitTypeQueries.fillListBoxItems(lstNBT, _unitType);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void loadUnits(int _comID)
    {
        try
        {
            ProcessUnitQueries.fillListBox(lstNBT, _comID);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void loadUser(int _comID, bool allowInsert)
    {
        try
        {
            User _user = new User();
            _user.ComID = _comID;
			if (allowInsert) 
			{
	            ProcessUser.fillDropDownItems1(_ddlUser, _user);
			}
			else
			{				
	            ProcessUser.fillDropDownItems(_ddlUser, _user);
			}
          
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw new Exception(ex.Message);
        }
    }

    private void loadGroup(int _comID)
    {
        try
        {
            UserGroup _userGroup = new UserGroup();
            _userGroup.ComID = _comID;
            ProcessUserGroup.fillDropDownItems(_ddlGroup, _userGroup);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void loadSecurityQuestion()
    {
        try
        {
            ProcessUser.fillDropDownSecurityQuestions(_ddlSecurityQuestion);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    private bool isValidData()
    {
        string _strSQL = "SELECT * FROM tblUser WHERE login = '" + _txtLoginName.Text +
                         "' AND comID = " + Session["trkCompany"].ToString() + ";";

         if (_txtLoginName.Text == "")
        {
            _lblMessage.Text = "Please Enter Login Name.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        else if (Utility.isExist(_strSQL))
        {
            if (_btnSave.Text == "Save")
            {
                _lblMessage.Text = "Login Name Exists.";
                _lblMessage.ForeColor = System.Drawing.Color.Red;
                return false;
            }
            else
            {

            }
        }
        else if (_ddlGroup.SelectedIndex < 1)
        {
            _lblMessage.Text = "Please Select Group.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
         else if (_ddlSecurityScheme.SelectedIndex < 1)
         {
             _lblMessage.Text = "Please Select Security Scheme.";
             _lblMessage.ForeColor = System.Drawing.Color.Red;
             return false;
         }

        return true;
    }


    private void saveUser()
    {
        User _user = new User();
        ProcessUser _processUser = new ProcessUser(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
        string password = _txtPassword.Text;
        try
        {
            _user.Login = _txtLoginName.Text;
            _user.UserName = _txtFullName.Text;
            _user.ComID = int.Parse(Session["trkCompany"].ToString());
            _user.GroupID = int.Parse(_ddlGroup.SelectedValue);
            _user.Email = _txtEmail.Text;
            _user.IsActive = _chkIsActive.Checked;
            _user.RoleID = 3;
            _user.Password = EncDec.GetEncryptedText(password);
            _user.SecurityQuestion = int.Parse(_ddlSecurityQuestion.SelectedValue);
            _user.SecurityScheme = int.Parse(_ddlSecurityScheme.SelectedValue);
            _user.SecurityAnswer = _txtSecurityA.Text;

            _processUser.UserObj = _user;
            _processUser.invoke();

            uID = _processUser.UID;

            Mailer.SendWelcomeMail(_user.Email, _user.Login, password, Session["trkCompany"].ToString()); 

            clearControls();

            _lblMessage.Text = "User Created Successfully.";
            _lblMessage.ForeColor = System.Drawing.Color.Green;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _user = null;
            _processUser = null;
        }
    }

    private void updateUser()
    {
        ProcessUser _processUser = new ProcessUser(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
        User _userObj = new User();
        try
        {

            _userObj.UID = int.Parse(_ddlUser.SelectedValue);
            _userObj.GroupID = int.Parse(_ddlGroup.SelectedValue);
            _userObj.Password = EncDec.GetEncryptedText(_txtPassword.Text);
            _userObj.UserName = _txtFullName.Text;
            _userObj.Email = _txtEmail.Text;
            _userObj.SecurityQuestion = int.Parse(_ddlSecurityQuestion.SelectedValue);
            _userObj.SecurityAnswer = _txtSecurityA.Text;
            _userObj.SecurityScheme = int.Parse(_ddlSecurityScheme.SelectedValue);
            _userObj.IsActive = _chkIsActive.Checked;

            _processUser.UserObj = _userObj;
            _processUser.invoke();


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        finally
        {
            _processUser = null;
            _userObj = null;
        }
    }

    private void updateUserGroup()
    {
        string _listValue = _hListValue.Value;
        string[] _listItem = _listValue.Split(';');
        if (_listItem.Length > 1)
        {
            ProcessUnitUserWise _processUnitUserWise = new ProcessUnitUserWise(AlarmasABC.BLL.InvokeOperations.operations.DELETE);
            try
            {
                _processUnitUserWise.UserID = int.Parse(_ddlUser.SelectedValue);
                _processUnitUserWise.invoke();
                _lblMessage.ForeColor = System.Drawing.Color.Green;
                _lblMessage.Text = "Update Successful";
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                _processUnitUserWise = null;
            }
        }

        ProcessUnitUserWise _UnitUserWise = new ProcessUnitUserWise(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
        try
        {
            _UnitUserWise.UserID = int.Parse(_ddlUser.SelectedValue);
            _UnitUserWise.GroupID = int.Parse(_ddlGroup.SelectedValue);
            _UnitUserWise.invoke();
            _lblMessage.ForeColor = System.Drawing.Color.Green;
            _lblMessage.Text = "Update Successful";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
        finally
        {
            _UnitUserWise = null;
        }

        if (_listItem.Length > 1)
        {
            if (_rdoUnits.Checked)
            {

                for (int i = 0; i < _listItem.Length - 1; i++)
                {
                    ProcessUnitUserWise _UntUsrWsIns = new ProcessUnitUserWise(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
                    try
                    {
                        _UntUsrWsIns.UnitID = int.Parse(_listItem[i].ToString());
                        _UntUsrWsIns.UserID = int.Parse(_ddlUser.SelectedValue);
                        _UntUsrWsIns.invoke();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                    finally
                    {
                        _UntUsrWsIns = null;
                    }

                }
            }
            else if (_rdoUnitGroup.Checked)
            {
                ProcessUserWiseUnitCat _UntUsrWsCat = new ProcessUserWiseUnitCat(AlarmasABC.BLL.InvokeOperations.operations.DELETE);
                try
                {
                    _UntUsrWsCat.UserID = int.Parse(_ddlUser.SelectedValue);
                    _UntUsrWsCat.invoke();
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                finally
                {
                    _UntUsrWsCat = null;
                }


                for (int i = 0; i < _listItem.Length - 1; i++)
                {
                    ProcessUserWiseUnitCat _UnitGroup = new ProcessUserWiseUnitCat(AlarmasABC.BLL.InvokeOperations.operations.UPDATE);
                    try
                    {
                        _UnitGroup.ComID = int.Parse(Session["trkCompany"].ToString());
                        _UnitGroup.GroupID = int.Parse(_listItem[i].ToString());
                        _UnitGroup.UserID = int.Parse(_ddlUser.SelectedValue);
                        _UnitGroup.invoke();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                    }
                    finally
                    {
                        _UnitGroup = null;
                    }
                }


            }
        }

    }


    private bool chkValidUpdateData()
    {
       if (_txtLoginName.Text == "")
        {
            _lblMessage.Text = "Please Enter Login Name.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        else if (_ddlGroup.SelectedIndex < 1)
        {
            _lblMessage.Text = "Please Select Group.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }

        else if (_ddlUser.SelectedIndex < 1)
        {
            _lblMessage.Text = "Please Select User.";
            _lblMessage.ForeColor = System.Drawing.Color.Red;
            return false;
        }
       else if (_ddlSecurityQuestion.SelectedIndex < 1)
       {
           _lblMessage.Text = "Please Select Security Question";
           _lblMessage.ForeColor = System.Drawing.Color.Red;
           return false;
       }

        return true;
    }

    protected void _btnSave_Click(object sender, EventArgs e)
    {
        if (isValidData())
        {
            if (_btnSave.Text == "Save")
            {
                saveUser();
                saveUnits();
                clearControls();
            }

            else if (_btnSave.Text == "Update" && chkValidUpdateData())
            {
                updateUser();
                int _comID = int.Parse(Session["trkCompany"].ToString());
				if ((bool)Session["fullAccess"] == true)
				{
	                updateUserGroup();
                	loadUnitType(_comID);
				}
				else
				{
					_lblMessage.Text = "User update successful";
				}

                clearControls();
                _ddlUser.SelectedIndex = 0;
                _btnSave.Text = "Save";
                _chkIsActive.Checked = true;
            }


        }

    }

    private void saveUserGroupWiseUnits()
    {
        ProcessUserGroupUnits _UserGroupUnits = new ProcessUserGroupUnits(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

        try
        {
            _UserGroupUnits.ComID = int.Parse(Session["trkCompany"].ToString());
            _UserGroupUnits.GroupID = int.Parse(_ddlGroup.SelectedValue);
            _UserGroupUnits.invoke();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        finally
        {
            _UserGroupUnits = null;
        }
    }

    private void saveUnits()
    {
        string _listValue = _hListValue.Value;
        string[] _listItem = _listValue.Split(';');

        if (_rdoUnitGroup.Checked)
        {
            ProcessUserGroupUnits _processGroup = new ProcessUserGroupUnits(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
            try
            {
                for (int i = 0; i < _listItem.Length - 1; i++)
                {
                    _processGroup.ComID = int.Parse(Session["trkCompany"].ToString());
                    _processGroup.GroupID = int.Parse(_listItem[i].ToString());
                    _processGroup.UserID = uID;
                    _processGroup.invoke();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                _processGroup = null;
            }
        }
        else if (_rdoUnits.Checked)
        {
            ProcessUsersUnitData _units = new ProcessUsersUnitData();
            try
            {
                for (int i = 0; i < _listItem.Length - 1; i++)
                {
                    _units.UnitID = int.Parse(_listItem[i].ToString());
                    _units.ComID = int.Parse(Session["trkCompany"].ToString());
                    _units.invoke();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                _units = null;
            }
        }
    }

    protected void _btnCancel_Click(object sender, EventArgs e)
    {

    }
    //protected void _lnkCreateUser_Click(object sender, EventArgs e)
    //{
    //    if (_lnkCreateUser.Text == "Create New User")
    //    {
    //        _lnkCreateUser.Text = "Update User";
    //        _btnSave.Text = "Save";
    //        _divUser.Visible = false;
    //        clearControls();
    //    }
    //    else if (_lnkCreateUser.Text == "Update User")
    //    {
    //        _lnkCreateUser.Text = "Create New User";
    //        _btnSave.Text = "Update";
    //        _divUser.Visible = true;
    //        clearControls();
    //    }
    //}

    protected void _ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_ddlUser.SelectedValue != "0")
        {
			if ((bool)Session["edit"]) 
			{
           		_btnSave.Text = "Update";
				_btnSave.Visible = true;
			}
			else
			{
				_btnSave.Visible = false;
			}

            //_divUser.Visible = true;
            clearControls();
            _lblMessage.Text = "";
            loadUserInfo(int.Parse(_ddlUser.SelectedValue));
            if (_rdoUnitGroup.Checked)
            {
                _loadUnderRdoUnitGroup();
            }
            if (_rdoUnits.Checked)
            {
                _loadUnderRdoUnit();
            }

        }
        else
        {
            int _comID = int.Parse(Session["trkCompany"].ToString());
            loadUnitType(_comID);
			if ((bool)Session["insert"])
			{
		        _btnSave.Text = "Save";
				_btnSave.Visible = true;
				_btnCancel.Text = "Cancel";
			}
            clearControls();
            _lblMessage.Text = "";
        }


    }

    private void _loadUnderRdoUnit()
    {
        ProcessUserWiseUnitCat _proUnit = new ProcessUserWiseUnitCat();
        lstBT.Items.Clear();
        lstNBT.Items.Clear();
        try
        {
            _proUnit.UserID = int.Parse(_ddlUser.SelectedValue);
            _proUnit.ComID = int.Parse(Session["trkCompany"].ToString());
            _proUnit.fillListedUnits(lstBT);
            _proUnit.fillNotListedUnits(lstNBT);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _proUnit = null;
        }
    }

    private void _loadUnderRdoUnitGroup()
    {

        ProcessUserWiseUnitCat _prounitgroup = new ProcessUserWiseUnitCat();
        lstBT.Items.Clear();
        lstNBT.Items.Clear();
        try
        {
            _prounitgroup.UserID = int.Parse(_ddlUser.SelectedValue);
            _prounitgroup.ComID = int.Parse(Session["trkCompany"].ToString());
            _prounitgroup.fillListedUnitGroups(lstBT);
            _prounitgroup.fillNotListedUnitGroups(lstNBT);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _prounitgroup = null;
        }
    }

    private void loadUserInfo(int _uID)
    {
        User _userObj = new User();
        try
        {
           
            _userObj.UID = _uID;
            ProcessUser _processUser = new ProcessUser(AlarmasABC.BLL.InvokeOperations.operations.SELECT);
            _processUser.UserObj = _userObj;
            _processUser.invoke();
            DataSet _ds = _processUser.Ds;

            _txtLoginName.Text = _ds.Tables[0].Rows[0]["login"].ToString();
            _txtFullName.Text = _ds.Tables[0].Rows[0]["userName"].ToString();
            _txtEmail.Text = _ds.Tables[0].Rows[0]["email"].ToString();
            string _pass = EncDec.GetDecryptedText(_ds.Tables[0].Rows[0]["password"].ToString());
            _txtPassword.Attributes.Add("value", _pass);
            _txtConfirmPassword.Attributes.Add("value", _pass);
            _ddlGroup.SelectedValue = _ds.Tables[0].Rows[0]["groupID"].ToString();
            _ddlSecurityQuestion.SelectedValue = _ds.Tables[0].Rows[0]["securityQuestion"].ToString();
            _txtSecurityA.Text = _ds.Tables[0].Rows[0]["securityAnswer"].ToString();
            _chkIsActive.Checked = bool.Parse(_ds.Tables[0].Rows[0]["isActive"].ToString());

            if (_ds.Tables[1].Rows.Count > 0) 
            {
                _ddlSecurityScheme.SelectedValue = _ds.Tables[1].Rows[0]["schemeID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            _userObj = null;
        }
    }

    private void clearControls()
    {
        _txtConfirmPassword.Attributes.Add("value", "");
        _txtPassword.Attributes.Add("value", "");
        _txtEmail.Text = "";
        _txtFullName.Text = "";
        _txtLoginName.Text = "";
        _txtSecurityA.Text = "";
        if (_ddlGroup.Items.Count > 0)
            _ddlGroup.SelectedIndex = 0;
        //if (_ddlUser.Items.Count > 0)
        //    _ddlUser.SelectedIndex = 0;
        if (_ddlSecurityQuestion.Items.Count > 0)
            _ddlSecurityQuestion.SelectedIndex = 0;
        _ddlSecurityScheme.SelectedIndex = 0;
        //_lblMessage.Text = "";
        lstBT.Items.Clear();
        //lstNBT.Items.Clear();
    }

    protected void _rdoUnitGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdoUnitGroup.Checked)
            //loadUnitType(int.Parse(Session["trkCompany"].ToString()));
            _loadUnderRdoUnitGroup();
    }
    protected void _rdoUnits_CheckedChanged(object sender, EventArgs e)
    {
        if (_rdoUnits.Checked)
            //loadUnits(int.Parse(Session["trkCompany"].ToString()));
            _loadUnderRdoUnit();
    }

    private void Update()
    {
        //_lnkCreateUser.Text = "Update User";
        _btnSave.Text = "Save";
        _divUser.Visible = false;
    }

    private void Create()
    {
        //_lnkCreateUser.Text = "Create New User";
        _btnSave.Text = "Update";
        _divUser.Visible = true;
    }
}
