<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewAccount.aspx.cs" Inherits="CreateNewAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create a New User - XtremeK DEVELOPMENT Tracking System</title>
    <link href="CSS/XtremeKLogin.css" rel="stylesheet" type="text/css" />
</head>
<body>
<form id="form1" runat="server">

	<div class="page">

		<div class="form_wrapper">
			<div class="form_header"></div> 

				<div class="form_center">
				<h1>
					Create A New Account
				</h1>
	
				<div class="form_field">
					<div class="form_label">
						First Name:
					</div>
					<div>
						<asp:TextBox ID="txtFirstName" CssClass="form_text_box" runat="server" Width="340px"></asp:TextBox>
					</div>
				</div>

				<div class="form_field">
					<div class="form_label">
						Last Name:         
					</div>
					<div>
						<asp:TextBox ID="txtLastName" CssClass="form_text_box" runat="server" Width="340px"></asp:TextBox>
					</div>
				</div>
				<div class="form_field">
					<div class="form_label">
						Login Name:         
					</div>
					<div>
						<asp:TextBox ID="txtLogin" CssClass="form_text_box" runat="server" Width="340px"></asp:TextBox>
					</div>
				</div>
				<div class="form_field">
					<div class="form_label">
						Password:         
					</div>
					<div>
						<asp:TextBox ID="txtPassword" runat="server" Width="340px" TextMode="Password" MaxLength="15" CssClass="form_text_box"></asp:TextBox>
						<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="txtPassword"
							ErrorMessage="Password is required." ToolTip="Password is required.">*</asp:RequiredFieldValidator>
					 </div>
				</div>
				<div class="form_field">
					<div class="form_label">
						Confirm Password:         
					</div>
					<div>
						<asp:TextBox ID="txtConfirmPassword" runat="server" Width="340px" TextMode="Password" MaxLength="15" CssClass="form_text_box"></asp:TextBox>
						<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="txtConfirmPassword"
							ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required.">*</asp:RequiredFieldValidator>
					 </div>
				</div>
				<div class="form_field">
					<div class="form_label">
						Contact Email:
					</div>
			  		<div>
						<asp:TextBox ID="txtEmail" CssClass="form_text_box" runat="server" Width="340px"></asp:TextBox>
					</div>
				</div>

				<div class="form_field">
					<div class="form_label">
						Unit Serial Number:
					</div>
			  		<div>
						<asp:TextBox ID="txtUnitSerial" CssClass="form_text_box" runat="server" Width="340px"></asp:TextBox>
					</div>
				</div>

				<div class="form_field">
					<div class="form_label">
						Security Question:
					</div>
			  		<div>
                        <asp:DropDownList ID="ddlSecurityQuestion"  Width="340px" runat="server" AutoPostBack="false"></asp:DropDownList>
					</div>
				</div>

				<div class="form_field">
					<div class="form_label">
						Security Answer:
					</div>
			  		<div>
						<asp:TextBox ID="txtSecurityAnswer" CssClass="form_text_box" runat="server" Width="340px"></asp:TextBox>
					</div>
				</div>

			   	<div class="form_field">
					<div class="form_label">
						Notes/Comments:
					</div>
			  		<div>
				    	<asp:TextBox ID="txtNotesComments" runat="server" Width="340px" Rows="6" CssClass="form_text_area" TextMode="MultiLine" ></asp:TextBox>
					</div>
			 	</div> 

				<br/>

				<div class="form_field">
					<div>
						<asp:ImageButton ID="btnOk" runat="server" ImageUrl="Images/submit.png" OnClick="_btnOk_Click"/>
					</div>
				</div>

				<br/>
				<div class="form_field">
					<div>
				 		<asp:Label ID="lblMessage" CssClass="form_label" runat="server"></asp:Label>
					</div> 
				</div>

				</div>

				<br/><br/>


				<div class="form_link">
					<a href="Login.aspx" ><< Return to Login Page</a>
				</div>

				<br/>

				<div class="form_footer"></div> 
			</div>
		</div>
    </form>
</body>
</html>
