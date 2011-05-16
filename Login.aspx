<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>XtremeK Login Page</title>
    <!--<link href="CSS/XtremeKLogin.css" rel="stylesheet" type="text/css" />-->
	<link href="CSS/style.css" rel="stylesheet" type="text/css" />
	<link href="CSS/top-bar.css" rel="stylesheet" type="text/css" />

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
     
	<div class="main">
    <div class="header">
	    <div id="logo">
	    	<a href="index.html"><img src="Images/guard-dog-logo.png" alt="Guard Dog Logo" border="0"/></a>
	    </div>
		  <div class="slogan2"><img src="Images/smart-reliable.png" alt="Smart, Reliable, Easy to Install"/></div>
		  <p><br />
	       <br />
	       <br />
	       <br />
	       <br />
	    </p>
	    <p>&nbsp;</p>
	    <div id="menucontainer">
	 	    <ul id="css3menu1" class="topmenu">
		      <li class="topfirst"><a href="http://guarddoggps.com" style="width:82px;height:15px;line-height:15px;">HOME</a></li>
		      <li class="topmenu"><a href="#" style="width:156px;height:15px;line-height:15px;"><span>PRODUCTS</span></a>
		        <ul>
			        <li class="subfirst"><a href="http://guarddoggps.com/auto-trac.html">AutoTrac®</a></li>
			        <li><a href="http://guarddoggps.com/fleet-trac.html">FleetTrac®</a></li>
			        <li><a href="http://guarddoggps.com/moto-trac.html">MotoTrac®</a></li>
			        <li><a href="http://guarddoggps.com/marine-trac.html">MarineTrac®</a></li>
		        </ul>
		      </li>
		      <li class="topmenu"><a href="http://guarddoggps.com/faq.html" style="width:56px;height:15px;line-height:15px;">FAQ</a></li>
		      <li class="topmenu"><a href="http://store.guarddoggps.com" style="width:93px;height:15px;line-height:15px;">STORE</a></li>
		      <li class="topmenu"><a href="http://guarddoggps.com/about.html" style="width:145px;height:15px;line-height:15px;">ABOUT US</a></li>
		      <li class="topmenu"><a href="http://guarddoggps.com/contact.html" style="width:131px;height:15px;line-height:15px;">CONTACT</a></li>
		      <li class="toplast"><a href="http://www.xtremek.com" title="ACTIVATE" style="width:134px;height:15px;line-height:15px;">ACTIVATE</a></li>
	      </ul>
	    </div>
	  </div>
	  <div>
      <div id="center">
        <div class="field">
          <label>User name</label>
			    <asp:TextBox ID="txtLogin" Width="150px" CssClass="form_text_box" runat="server"></asp:TextBox>
        </div>

        <div class="field">
          <label>Password</label>					    
          <asp:TextBox ID="txtPassword" Width="150px" CssClass="form_text_box" runat="server" TextMode="Password"></asp:TextBox>
      	</div>
	   
        <div class="field">
          <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="Images/submit.png" OnClick="btnLogin_Click"/>
        </div>

      </div>
    </div>
	  <div class="footer"><div class="footer-left">Copyright © 2011 Guard Dog. All rights reserved.</div>
	    <div id="socialmedia"><a href="http://www.facebook.com/pages/Guard-Dog-GPS/118770118204831"><img src="Images/facebook.png" alt="Find Us" border="0"/></a><a href="http://twitter.com/#!/GuardDogGPS"><img src="Images/twitter.png" alt="Follow Us" border="0"/></a></div>
	  </div>
  </div>

    <%--
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

					</div>
					<div class="login_page_label"></div>
					<div class="login_page_tfield">					

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
    --%>
</form>
</body>
</html>
