<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeFile="ContactUs.aspx.cs" Inherits="CompanyAdmin_ContactUs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Company.css" rel="stylesheet" type="text/css" />
     <link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="formWrap">
       <div class="container_01">
            <div>
            	<label class="fullWidth">Type in your email address and select a department to contact, type in your message or question, then click Submit.</label>            
         	</div>
         </div>
       <div class="container_01">
            <div class="container_02">
                <div>
                    <label class="form_label">
                        Your Name:
                    </label>
                
                </div>
                <div>
                
                <asp:TextBox ID="_txtUserName" runat="server" CssClass="form_TextBox"></asp:TextBox>
                
                </div>
            </div>
            
        </div>
       <div class="container_01">
            <div class="container_02">
                <div>
                    <label class="form_label">
                        Email:
                    </label>
                </div>
                 <div>
                    <asp:TextBox ID="_txtEmail" runat="server" CssClass="form_TextBox"></asp:TextBox>
                </div>
            </div>
         
        </div>
       <div class="container_01">
            <div class="container_02">
                <div>
                    <label class="form_label">
                        Department:
                    </label>
                    
                </div>
                <div>
                
                    <asp:DropDownList ID="_ddlDepartment" runat="server" CssClass="form_ddl">
                          <asp:ListItem>Select Department</asp:ListItem>
                          <asp:ListItem>Technical Support</asp:ListItem>
                          <asp:ListItem>Sales</asp:ListItem>
                          <asp:ListItem>Inquiry</asp:ListItem>
                    </asp:DropDownList>
                
            </div>
            </div>
           
            
         </div>
       <div class="container_01">
                 <div>
                    <label class="form_label">
                        Message or Question:
                    </label>
                </div>
                
                <div>
                <asp:TextBox ID="_tbDesc" runat="server" Width="368px" Rows="6" CssClass="form_TextArea" TextMode="MultiLine" ></asp:TextBox>
                </div>   
                
       </div> 
        
       <div class="container_01">
           <div class="container_02">
                <div>
                    <asp:Button ID="_btnSubmit" CssClass="form_SubmitBtn" runat="server" Text="Submit" 
                        onclick="_btnSubmit_Click" /> 
                     <asp:Button ID="_btnReset" runat="server" Text="Reset" 
                        CssClass="form_SubmitBtn" onclick="_btnReset_Click" />
                </div>
                <div>
                              
                
                
               </div>
           </div>
            
        </div>
       
       <div>
           <asp:Label ID="_lblMessage" runat="server" Text=""></asp:Label>
           
           <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
               ErrorMessage="Please enter valid email ID" ControlToValidate="_txtEmail" ValidationExpression="^[\w-\.]{1,}\@([\da-zA-Z-]{1,}\.){1,}[\da-zA-Z-]{2,6}$"></asp:RegularExpressionValidator>
           
       </div>
       
    </div>
    </form>
</body>
</html>
