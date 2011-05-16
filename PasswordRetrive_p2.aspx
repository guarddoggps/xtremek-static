<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordRetrive_p2.aspx.cs" Inherits="CompanyAdmin_PasswordRetrive_SQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recover Lost Password - XtremeK DEVELOPMENT Tracking System</title>
    <link href="CSS/XtremeKLogin.css" rel="stylesheet" type="text/css" />
	<style type="text/css"> 
        html,body,form 
        { 
            height:100%;
        } 
    </style>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="_scriptManager" runat="server"  AsyncPostBackTimeout="300"></asp:ScriptManager>

	<div class="page"> 
		<div class="form_wrapper">
			<div class="form_header"></div> 

			<div class="form_center">
				<h1>
					Retrieve Lost Password
				</h1>

				<div class="form_field">
					<p class="form_paragraph">
                        Please answer the security question so we can send you your password.
                    </p>
				</div>

				<div class="form_field">
					<div class="form_label">
						Security Question:
					</div>
					<div style="font-size: 0.9em; font-weight: bold;">
						<asp:Label ID="lblQuestion" runat="server"></asp:Label>
					</div> 
				</div>                

				<br/>

				<div class="form_field">
					<div class="form_hlabel">
						Enter Security Answer
					</div>
					<div>
						<asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>
					</div>
				</div>
				
				<br/>

				<div style="float: left;" class="form_field">
					<asp:Button ID="btnBack" runat="server" Text="<< Back" 
		                        onclick="btnBack_Click" />
				</div>

				<div style="float: right; margin-right: 36px;" class="form_field">
					<asp:Button ID="btnOk" runat="server" Text="Retrieve Password" 
		                        onclick="btnOk_Click" />
				</div>
				
				<br/><br/>
				<div class="form_field">
					<asp:Label ID="lblMessage" CssClass="form_label" runat="server"></asp:Label>
				</div>
			</div>

			<br/><br/>


			<div class="form_link">
				<a href="Login.aspx" ><< Return to Login Page</a>
			</div>

  			<div style="clear: both;">&nbsp;</div>
			<div class="form_footer"></div> 
        </div>
        
        <div class="side_c">
        </div>
    </div> 
</form>
</body>
</html>
