<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemoteControl.aspx.cs" Inherits="XtremeK_RemoteControl" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Control Units Remotely</title>
    <link href="CSS/ajaxstyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    	 html, body,form {
            margin: 0;
            padding: 0;
            width: 380px;
            background-color:#f5f5f5;
		}
    	
        .cell_label2
        {
            margin-left: 10px;
        }
        .button
        {
            width: 60px;
            height: 86px;
            margin: 0px 6px 0px 6px;
            float: left;
        }
        .button:hover
        {
            background: url("Images/RemoteControl/hover-back.png") no-repeat;
        }

        .close_button
        {
            width: 32px;
            height: 32px;
            float: right;
            position: relative;
            top: -20px;
            right: 32px;
        }

        .form_header_label
        {
            font-family:Trebuchet MS;
            font-size: 14px;
            font-weight: bold;
            float: left;
            color: white;
            padding-top: 2px;
            padding-left: 12px;
        }
        
        .topbar
        {
        	background-color: black; 
        	width: 380px;
        	height: 24px;
        }

        .message_label
        {
            font-family:Trebuchet MS;
            font-size: 12px;
            margin-top: 16px;
        }

        .status_label
        {
            font-family:Trebuchet MS;
            font-size: 13px;
            color: white;
        }

        .popup_top
        {
            background: url(Images/RemoteControl/popup-top.png) no-repeat;
            width: 358px;
            height: 35px;
            position: relative;
            top: 0px;
            left: 2px;
        }

        .popup
        {
            background: url(Images/RemoteControl/popup-middle.png) repeat-y;
            width: 358px;
            float: left;
        }

        .popup_bottom
        {
            background: url(Images/RemoteControl/popup-bottom.png) no-repeat;
            width: 358px;
            height: 35px;
            float: left;
            position: relative;
            bottom: 0px;
            left: 0px;
        }

        .submit_button
        {
            float: right;
            margin: 8px 0px 8px 8px;
            border: 1px solid white;
            background-color:gray;
            height: 22px;
            font-family: Trebuchet MS;
            font-size: 13px;
            font-weight: normal;
            color: white;
            -moz-border-radius: 5px; 
            -webkit-border-radius: 5px;
            -khtml-border-radius: 5px;  
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div style="width: 380px;">
        <div class="topbar">
        	<asp:Label runat="server" ID="unitName" CssClass="form_header_label">Unit</asp:Label>
        </div>  
    
        <div style="background: url(Images/RemoteControl/form.jpg) no-repeat; float: left; width: 380px; height: 371px;">
              

            <asp:UpdatePanel ID="updatePanel01" runat="server">
                <ContentTemplate>      
                    <div style="position: absolute; top: 62px; left: 22px; width: 176px;">
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/lock.png" OnClick="btnLock_Click" AutoPostBack="true"/>                        
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/unlock.png" OnClick="btnUnlock_Click" AutoPostBack="true"/>
                    </div>    
                    <div style="position: absolute; top: 62px; left: 210px; width: 176px;">
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/silent-alarm.png" OnClick="btnSilentAlarm_Click" AutoPostBack="true"/>
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/panic-alarm.png" OnClick="btnPanicAlarm_Click" AutoPostBack="true"/>
                    </div>
                    <div style="position: absolute; top: 210px; left: 22px; width: 176px;">
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/stop-engine.png" OnClick="btnStopEngine_Click" AutoPostBack="true"/>        
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/start-engine.png" OnClick="btnStartEngine_Click" AutoPostBack="true"/>
                    </div>
                    <div style="position: absolute; top: 210px; left: 210px; width: 176px;">
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/auxilary.png" OnClick="btnAuxilary_Click" AutoPostBack="true"/>                           
                        <asp:ImageButton runat="server" CssClass="button" ImageUrl="Images/RemoteControl/status.png" OnClick="btnStatus_Click" AutoPostBack="true"/>
                    </div>

                    <asp:Timer ID="Timer" runat="server" Interval="15000" OnTick="Timer_Tick"></asp:Timer>

                    <div style="position: absolute; top: 358px; left: 26px; width: 380px;">
                        <asp:Label ID="lblMessage" CssClass="message_label" runat="server"></asp:Label>
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="updatePanel02" runat="server">
                <ContentTemplate>     
                    <div id="status_div" runat="server" style="position: relative; top: 16px; left: 36px;float: left; width: 360px; height: 380px;">

                        <div class="popup_top"></div>
                            <div style="height: 252px" class="popup">
                            <asp:ImageButton runat="server" CssClass="close_button" ImageUrl="Images/RemoteControl/close.png" OnClick="btnClose_Click" AutoPostBack="true"/>

                            <div style="float: left; position: relative; left: 54px; top: 18px; width: 256px;">
                                <asp:Label runat="server" ID="unitInfo" CssClass="status_label"></asp:Label>
                            </div>
                        </div>

                        <div class="popup_bottom"></div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="updatePanel03" runat="server">
                <ContentTemplate>     
                    <div id="password_div" runat="server" style="position: relative; top: 90px; left: 36px; float: left; width: 360px; height: 152px;">

                        <div class="popup_top"></div>
                        <div style="height: 96px" class="popup">
                            <img style="margin-left: 52px;" src="Images/RemoteControl/enter-pass.png" />
                            <asp:ImageButton runat="server" CssClass="close_button" ImageUrl="Images/RemoteControl/close.png" OnClick="btnClose_Click" AutoPostBack="true"/>

                            <div style="float: left; position: relative; left: 64px; top: 22px;">
                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="80"  Width="226" CssClass="form_TextBox" TextMode="Password"></asp:TextBox><br/>
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="submit_button" OnClick="btnSubmit_Click"></asp:Button>
                            </div>
                                
                        </div>

                        <div class="popup_bottom"></div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        </div>
    </div>
    </form>
</body>
</html>
