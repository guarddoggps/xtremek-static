<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Speeding.aspx.cs" Inherits="Tracking_Speeding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Speeding Alerts Setup</title>
    <link href="../CSS/SafetyZone.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" />
    <script src="../Js/NumericValidation.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript">
       

            function GetRadWindow()
            {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;//IE (and Moz az well)
                return oWindow;
            }


            function CloseOnReload()
            {
                GetRadWindow().Close();
            }
                      
         
         
</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="AlertsSetupScriptManager" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:HiddenField ID="haveData" runat="server" />
        <asp:HiddenField ID="rulesID" runat="server" />        
    </div>

    <div class="formWrap">
        <div class="container_01">
          <div class="container_02">
            <label>
                Enabled
            </label>
            <div class="cell_label">                
               <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                 <ContentTemplate>
                   <div>
                      <asp:CheckBox CssClass="alertcheckbox" ID="_chkOn" runat="server" 
                           AutoPostBack="True" oncheckedchanged="_chkOn_CheckedChanged" 
                      />
                   </div>
                   <div class="alertcell_label">
                        On
                   </div>
                   <div>
                      <asp:CheckBox CssClass="alertcheckbox" ID="_chkOff" runat="server" 
                           AutoPostBack="True" oncheckedchanged="_chkOff_CheckedChanged" 
                       />
                   </div>
                   <div class="alertcell_label">
                        Off
                   </div>                                        
                 </ContentTemplate>
               </asp:UpdatePanel>                                                  
            </div>
          </div>
        </div>  
           
      <div class="container_01">
        <div class="container_02">
          <label>
              Speed Value (In Mph)
          </label>
          <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="cell_label">    
                        <asp:TextBox CssClass="form_TextBox" ID="_txtSpeedValue" onkeypress="return(allownumbers(event))" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ErrorMessage="Please enter rules value." ControlToValidate="_txtSpeedValue"></asp:RequiredFieldValidator>
                    </div>
                </ContentTemplate>
             </asp:UpdatePanel>                    
          </div>
         </div>
       </div>        
       <div class="container_01">
          <div class="container_02">
            <label>
                Notification Email(s)
            </label>
            <div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="cell_label">
                            <asp:TextBox CssClass="form_TextBox" ID="_txtNotificationEmails" runat="server"></asp:TextBox>
                            &nbsp;
                        </div>                        
                    </ContentTemplate>
                </asp:UpdatePanel>                                    
            </div>
         </div>
        </div>
        
        <div class="container_01">
          <div class="container_02">
            <label>
                Notification Phone
            </label>
            <div>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <div class="cell_label">
                            <asp:TextBox CssClass="form_TextBox" ID="_txtPhoneNumber" runat="server"></asp:TextBox>                            
                        </div>                        
                    </ContentTemplate>
                </asp:UpdatePanel>                                    
            </div>
         </div>
        </div>
        <div class="container_01">
          <div class="container_02">
            <label>
                Enable SMS
            </label>
            <div class="cell_label">                
               <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                 <ContentTemplate>
                   <div>
                      <asp:CheckBox CssClass="alertcheckbox"  Checked="true" ID="_ckOnSMS" runat="server" 
                           AutoPostBack="True" oncheckedchanged="_ckOnSMS_CheckedChanged"/>
                   </div>
                   <div class="alertcell_label">
                        On
                   </div>
                   <div>
                      <asp:CheckBox CssClass="alertcheckbox" Checked="false" ID="_chkOffSMS" runat="server" 
                           AutoPostBack="True" oncheckedchanged="_chkOffSMS_CheckedChanged" />
                   </div>
                   <div class="alertcell_label">
                        Off
                   </div>                                        
                 </ContentTemplate>
               </asp:UpdatePanel>                                                  
            </div>
          </div>
        </div>
        
       <div class="container_01">
          <div class="container_02">
            <label>
                Message
            </label>
            <div >
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="cell_label">
                            <asp:TextBox CssClass="form_TextBox" ID="_txtMessage" runat="server"></asp:TextBox>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                                   
            </div>
         </div>
        </div>
       <div class="container_01">
          <div class="alertcontainer">
            <div class="alertinstruction">
                System will send one notification only when speed is over<br /> 
                preset value. Only one speed rule is allowed per unit.
            </div>
         </div>
        </div>
       <div class="container_01">
         <div class="container_02">
            <div class="alertbutton01">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="_btnOk" CssClass="form_SubmitBtn" Text="Ok" runat="server" 
                            onclick="OkButton_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                                       
            </div>
            
            <div class="alertbutton01">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="_btnExit" CssClass="form_SubmitBtn" runat="server" Text="Cancel" OnClientClick="CloseOnReload()"/>
                        </div>   
                    </ContentTemplate>
                </asp:UpdatePanel>                                       
            </div>
<%--            <div class="alertbutton01">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:Button ID="DeleteButton" CssClass="formSubmitBtn1" runat="server" Text="Delete Rule" 
                            onclick="DeleteButton_Click" Visible="False" />
                        </div>   
                    </ContentTemplate>
                </asp:UpdatePanel>                                       
            </div>--%>                        
         </div>
       </div>
       <div >
          <div class="alertcontainer">
            <div >
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <div class="alertinstruction">
                            <asp:Label ID="_lblMessage" runat="server" Text=""></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
         </div>
        </div>                         
      </div> 
      <asp:UpdateProgress ID="siteUpdateProgress" DisplayAfter="50"  runat="server">
         <ProgressTemplate>
            <div class="TransparentGrayBackground"></div>
            <div class="Sample5PageUpdateProgress">
                <asp:Image  ID="ajaxLoadNotificationImage" 
                            runat="server" 
                            ImageUrl="../Images/loading.gif" 
                            AlternateText="[image]" />
                &nbsp;Loading...
            </div>      
          </ProgressTemplate>
      </asp:UpdateProgress>
      
    </form>
</body>
</html>
