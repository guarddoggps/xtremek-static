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
using AlarmasABC.Core.Admin;
using AlarmasABC.BLL.ProcessUser;
using AlarmasABC.BLL.ProcessUserGroup;
using AlarmasABC.BLL.ProcessUnitManagement;
using AlarmasABC.BLL.ProcessUnitType;
using AlarmasABC.DAL.Queries;
using AlarmasABC.Utilities;
using AlarmasABC.BLL.ProcessCompany;

public partial class CreateNewAccount : System.Web.UI.Page
{
	private static int comID = 3;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ProcessUser.fillDropDownSecurityQuestions(ddlSecurityQuestion);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }

	private int createUserGroup(string name)
    {
        UserGroup userGroup = new UserGroup();
        ProcessUserGroup pGroup = new ProcessUserGroup(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
        try
        {
            userGroup.GroupName = name;
            userGroup.ComID = comID;

            pGroup.UserGroup = userGroup;
            pGroup.invoke();

			DataSet _ds = new DataSet();
			ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet("SELECT groupID FROM tblGroup WHERE comID = " 
								  + comID + " AND groupName = '" + name + "';");

			int groupID = int.Parse(_ds.Tables[0].Rows[0][0].ToString());
			if (groupID < 1)
			{
				return -1;
			}

			return groupID;
        }
        catch (Exception ex)
        {
            Console.WriteLine("CreateNewAccount::createUserGroup():" + ex.Message.ToString());
        }

        finally
        {
            userGroup = null;
            pGroup = null;
        }

		return -1;
    }

	private int createUnitGroup(string name)
	{
		AlarmasABC.Core.Admin.UnitType unitType = new AlarmasABC.Core.Admin.UnitType();

        ProcessUnitTypeNotQueries insert = new ProcessUnitTypeNotQueries(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

        try
        {
            unitType.TypeName = name;
            unitType.ComID = comID;

            insert.UnitType = unitType;
            insert.invoke();

			DataSet _ds = new DataSet();
			ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet("SELECT typeID FROM tblUnitType WHERE comID = " 
								  + comID + " AND typeName = '" + name + "';");

			int typeID = int.Parse(_ds.Tables[0].Rows[0][0].ToString());
			if (typeID < 1)
			{
				return -1;
			}

			return typeID;

        }
        catch (Exception ex)
        {
            throw new Exception(" CreateNewAccount::createUnitGroup(): " + ex.Message.ToString());
        }
        finally
        {
            unitType = null;
        }
	}

	private void createUnit(int deviceID, int typeID)
    {
        Units _units = new Units();
        ProcessUnitManagementQueries _unitManagement = new ProcessUnitManagementQueries(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

        try
        {
            _units.UnitID = deviceID;
            _units.UnitName = deviceID.ToString();
            _units.TypeID = typeID;
            _units.IconName = "redcar";
            _units.ComID = comID;

            _unitManagement.Units = _units;
            _unitManagement.invoke();

        	/*DataSet _ds = new DataSet();
			ExecuteSQL exec = new ExecuteSQL();
			_ds = exec.getDataSet("SELECT unitID FROM tblUnits WHERE deviceID = " + deviceID + 
								  " AND unitName = '" + deviceID + "' AND comID = " + comID);
            int unitID = int.Parse(_ds.Tables[0].Rows[0][0].ToString());
			if (unitID < 1) 
			{
				return -1;
			}
			
			return unitID;*/

        }
        catch (Exception ex)
        {
            throw new Exception("CreateNewAccount::createUnit(): " + ex.Message.ToString());
        }
        finally
        {
            _units = null;
            _unitManagement = null;
        }
    }

	private int getDefaultSecurityScheme()
	{
		ExecuteSQL exec = new ExecuteSQL();
		DataSet _ds = new DataSet();
		_ds = exec.getDataSet("SELECT id FROM tblSecurityScheme WHERE comID = " + comID + 
							  " AND defaultScheme = '1'"); 

		return int.Parse(_ds.Tables[0].Rows[0][0].ToString());
	}

	private void createUser(string fullName, string name, string password, string email,
						    int unitGroupID, int userGroupID, int securityQuestion, 
							string securityAnswer)
    {
        User _user = new User();
        ProcessUser _processUser = new ProcessUser(AlarmasABC.BLL.InvokeOperations.operations.INSERT);

        try
        {
            _user.Login = name;
            _user.UserName = fullName;
            _user.ComID = comID;
            _user.GroupID = userGroupID;
            _user.Email = email;
            _user.IsActive = true;
            _user.RoleID = 3;
            _user.Password = EncDec.GetEncryptedText(password);
            _user.SecurityQuestion = int.Parse(ddlSecurityQuestion.SelectedValue);
            _user.SecurityScheme = getDefaultSecurityScheme();
            _user.SecurityAnswer = securityAnswer;

            _processUser.UserObj = _user;
            _processUser.invoke();

            int uID = _processUser.UID;

			ProcessUserGroupUnits _processGroup = new ProcessUserGroupUnits(AlarmasABC.BLL.InvokeOperations.operations.INSERT);
            _processGroup.ComID = comID;
            _processGroup.GroupID = unitGroupID;
            _processGroup.UserID = uID;
            _processGroup.invoke();
           
            Mailer.SendWelcomeMail(_user.Email, _user.Login, password, comID.ToString());
        }
        catch (Exception ex)
        {
            throw new Exception("CreateNewAccount::createUser(): " + ex.Message.ToString());
        }
        finally
        {
            _user = null;
            _processUser = null;
        }
    }

	private bool isValid()
	{
		string msg = "";
		if (txtFirstName.Text.Length < 1)
		{
			msg = "Please insert you first name.";
		}
		if (txtLastName.Text.Length < 1)
		{
			msg = "Please insert your last name.";
		}
		if (txtPassword.Text.Length < 1)
		{
			msg = "Please enter a password.";
		}
		if (txtPassword.Text != txtConfirmPassword.Text)
		{
			msg = "The passwords you entered don't match.";
		}
		if (txtEmail.Text.Length < 1)
		{
			msg = "Please enter your email address.";
		}
		if (int.Parse(ddlSecurityQuestion.SelectedValue) == 0)
		{
			msg = "Please select a security scheme.";
		}
		if (txtSecurityAnswer.Text.Length < 1)
		{
			msg = "Please insert a security answer.";
		}

		if (msg.Length > 0)
		{
			lblMessage.ForeColor = System.Drawing.Color.Red;
			lblMessage.Text = msg;
			return false;
		}

		return true;
	}

	private bool userExists(string name, string email)
	{
		DataSet _ds = new DataSet();
		ExecuteSQL exec = new ExecuteSQL();
		_ds = exec.getDataSet("SELECT * FROM tblUser WHERE login = '" + name + "' AND" + 
							  " email = '" + email + "';");
		if (_ds.Tables[0].Rows.Count > 0) 
		{
			return true;
		}

		return false;
	}

	private bool unitExists(string deviceID)
	{
		DataSet _ds = new DataSet();
		ExecuteSQL exec = new ExecuteSQL();
		_ds = exec.getDataSet("SELECT * FROM tblUnits WHERE deviceID = " + deviceID);
		if (_ds.Tables[0].Rows.Count > 0) 
		{
			return true;
		}

		return false;
	}

    private void submitForm()
    {
        try
        {
			if (!isValid()) return;

			string fullName = txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
			string name = txtLogin.Text.Trim();
			string email = txtEmail.Text.Trim();
			string deviceID = txtUnitSerial.Text.Trim();

			// Now check if the users and units already exist
			if (userExists(name, email))
			{
				lblMessage.ForeColor = System.Drawing.Color.Red;
		        lblMessage.Text = "A user with the provided login name and " + 
									"email already exists.";
				return;
			}
			if (unitExists(deviceID))
			{
				lblMessage.ForeColor = System.Drawing.Color.Red;
		        lblMessage.Text = "A unit with the provided Unit Serial Number" +
									" already exists.";
				return;
			}

			int userGroupID = createUserGroup(name);
			int unitGroupID = createUnitGroup(name);
			createUnit(int.Parse(deviceID), unitGroupID);
			createUser(fullName, name, txtPassword.Text, email, unitGroupID, 
					   userGroupID, int.Parse(ddlSecurityQuestion.SelectedValue),
					   txtSecurityAnswer.Text);

			string msg;

			ProcessCompanyQueries processCompany = new ProcessCompanyQueries(comID);
			processCompany.invoke();
			DataSet _ds = processCompany.Ds;

			string companyName = _ds.Tables[0].Rows[0]["companyName"].ToString();
			msg = "A user account has been created in the " + companyName + " application.\r\n";
			msg += "Here is the user information: \r\n\r\n";
			msg += "Full Name: " + fullName + "\r\n";
			msg += "Login: " + name + "\r\n";
			msg += "Password: " + txtPassword.Text + "\r\n";
			msg += "Email: " + email + "\r\n";
			msg += "Unit ID: " + deviceID + "\r\n";
			msg += "Security Question: " + ddlSecurityQuestion.Text + "\r\n";
			msg += "Security Answer: " + txtSecurityAnswer.Text + "\r\n";
			msg += "Notes/Comments: " + txtNotesComments.Text + "\r\n\r\n";
			msg += "Other Info: \r\n";
			msg += " - A unit group (" + name + ") was created.\r\n";
			msg += " - A user group (" + name + ") was created.\r\n";
			msg += " - The unit group \"" + name + "\" was assigned to the user.\r\n";

	        Mailer.SendMailMessage("webmaster@xtremek.com", "support@alarmasabc.net", "", "",
								   "A user account has been created  in the " 
									+ companyName + " application", msg);

			lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "A new account has been created.\n" +
							" A message has been sent to the provided email address.";
        }
        catch (Exception ex)
        {
			lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "Could not create a new user.";
			Console.WriteLine("CreateNewAccount::submitForm(): " + ex.Message.ToString());
        }
        finally
        {

        }
    }


    protected void _btnOk_Click(object sender, EventArgs e)
    {
        submitForm();
    }
}
