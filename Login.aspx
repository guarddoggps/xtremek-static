<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>XtremeK Login Page</title>
    <link href="CSS/XtremeKLogin.css" rel="stylesheet" type="text/css" />

    <style type="text/css"> 

        html,body,form
        { 
            overflow: hidden;
            height:100%;
        } 
 
    </style>
    <script type="text/javascript">
     
		// Get the client time zone and set the hidden
		// field to the aquired value so the server can read
		// it upon postback.
		function getClientTimeZone()
		{
		    var date = new Date();
		    var tz = -date.getTimezoneOffset();
		    var tz_field =  document.getElementById('_timezone');
		    tz_field.value = tz;
		}

    </script>

</head>
<body onload="getClientTimeZone();">
<form id="form1" runat="server">

	<asp:HiddenField ID="_timezone" runat="server" />
     
    <div class="page"> 
        <div id="login_page_header">
            <a class="logo_abc" href="http://alarmasabc.net"></a>
            <div class="logo_xtremek"></div> 
        </div> 

        <div class="login_page_floater"></div> 
	
        <div class="login_page_wrap">
			<div class="login_page_form">
				<div class="login_page_center">
					<div class="login_page_label">
					    User name
					    
                    </div>
					<div class="login_page_tfield">
					    <asp:TextBox ID="txtLogin" Width="150px" CssClass="form_text_box" runat="server"></asp:TextBox>
					</div>
					<div class="login_page_label">
					    Password
					</div>
					<div class="login_page_tfield">
					    <asp:TextBox ID="txtPassword" Width="150px" CssClass="form_text_box" runat="server" TextMode="Password"></asp:TextBox>
					</div>
					<div class="login_page_label"></div>
					<div class="login_page_tfield">					
					    <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="Images/submit.png" OnClick="btnLogin_Click"/>
					</div>
					<div class="login_page_label"></div>
					<div class="login_page_tfield">
					    <a href="PasswordRetrive.aspx" >Forgot your password?</a>
					</div>
					<div style="margin-left: 0px; width: 80px;" class="login_page_label"></div>
					<div class="login_page_message">
					    <asp:Label ID="lblMessage" CssClass="form_label" runat="server"></asp:Label>
					</div>
					<div style="width: 84px" class="login_page_label"></div>
					<div class="login_page_tfield_link">
					    <a href="NewAccount.aspx" >Create new account</a>
					</div>

					<div class="login_page_terms">
						<div>By clicking "Submit" you agree to the following <a href="TermsAndConditions.aspx">Terms and Conditions</a></div>
					</div>
				</div>	
            </div>
        </div> 

    </div> 
</form>
</body>
</html>
