<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserUnitList.aspx.cs" Inherits="CompanyAdmin_UserUnitList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User & Unit List</title>
    <link href="../CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Company.css" rel="stylesheet" type="text/css" />
     <link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" /> 
     <link href="../CSS/ajaxstyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
             <asp:UpdateProgress ID="siteUpdateProgress" DisplayAfter="50"  runat="server">
                  <ProgressTemplate>
                
                      <div class="TransparentGrayBackground"></div>
                
                      <div class="Sample5PageUpdateProgress">
                          <asp:Image  ID="ajaxLoadNotificationImage" 
                                    runat="server" 
                                    ImageUrl="../Images/loading.gif" 
                                    AlternateText="[image]" />
                       
                      </div>
                  </ProgressTemplate>
              </asp:UpdateProgress>
            
        <div class="container_01">
            <div class="container_02">
              <label class="form_label">
                    Select User or Unit
              </label>
              <div class="cell_label" >
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div class="radiobutton">
                                    <asp:RadioButton ID="_rdoUser" runat="server" AutoPostBack="true" 
                                     GroupName="UserUnit" oncheckedchanged="_rdoUser_CheckedChanged" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel> 
                   <div class="rl_cell_label" >
                       User List
                   </div>
                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                            <ContentTemplate>
                                <div class="radiobutton">
                                    <asp:RadioButton ID="_rdoUnit" runat="server" AutoPostBack="true" 
                                     GroupName="UserUnit" oncheckedchanged="_rdoUnit_CheckedChanged" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                   <div class="rl_cell_label">
                      Units List   
                   </div> 
              </div>                  
            </div>
        </div>
    
    
    <div class="container_01">        
           
           <asp:UpdatePanel ID="_up"  runat="server">
                <ContentTemplate>
                    <telerik:RadGrid ID="_grdUnit" AllowAutomaticInserts="false" 
                                runat="server" GridLines="None" Skin="Default"  
                                AutoGenerateColumns="false" Width="100%" AllowPaging="false"
                                onneeddatasource="_DataSource"  PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                ondeletecommand="_grdUserUnit_DeleteCommand" AllowSorting="true" 
                        onitemdatabound="_grdUserUnit_ItemDataBound" 
                        onitemcommand="_grdUserUnit_ItemCommand">
                                
                    <ExportSettings>
                        
                        <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                        PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                    
                    </ExportSettings>
                    
                    <PagerStyle AlwaysVisible="true"/> 
                    <MasterTableView DataKeyNames="ID" CommandItemDisplay ="Top">            
                        <CommandItemSettings AddNewRecordText=""  />  
                        <Columns>
                          
                            <telerik:GridTemplateColumn HeaderText = "Unit Name">
                                <ItemTemplate>
                                        <asp:Label ID="_UnitName" runat="server" Text='<%# Eval("unitName") %>'></asp:Label>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText = "Type Name">
                                <ItemTemplate>
                                        <asp:Label ID="_TypeName"  runat="server" Text='<%# Eval("typeName") %>'></asp:Label>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                          
                           <%--<telerik:GridCheckBoxColumn DataField="Status" HeaderText="Status" UniqueName="Status"></telerik:GridCheckBoxColumn>--%>
                           
                           <telerik:GridTemplateColumn HeaderText = "Status" Visible="false">
                                <ItemTemplate>
                                        <asp:CheckBox ID="Status" runat="server" Checked='<%# Bind("status") %>'></asp:CheckBox>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           
                            <telerik:GridButtonColumn CommandName="Item" UniqueName="Disable"></telerik:GridButtonColumn>
                           <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you wish to delete?" Text="Delete" UniqueName="Delete"></telerik:GridButtonColumn>                                                      
                        </Columns>                        
                                                           
                    </MasterTableView>
                        
                    </telerik:RadGrid>
                    
                    <telerik:RadGrid   ID="_grdUser" AllowAutomaticInserts="true" 
                                runat="server"  GridLines="None" Skin="Default"  
                                AutoGenerateColumns="false" Width="100%" AllowPaging="false"
                                onneeddatasource="_DataSource"   PagerStyle-Mode="NextPrevNumericAndAdvanced"
                                ondeletecommand="_grdUserUnit_DeleteCommand" AllowSorting="true" 
                        onitemdatabound="_grdUserUnit_ItemDataBound" 
                        onitemcommand="_grdUserUnit_ItemCommand">
                                
                    <ExportSettings>
                        
                        <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                        PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                    
                    </ExportSettings>
                    
                    <MasterTableView DataKeyNames="ID" CommandItemDisplay ="Top">            
                        <CommandItemSettings AddNewRecordText=""  />  
                        <Columns>
                          
                            <telerik:GridTemplateColumn HeaderText = "Login Name">
                                <ItemTemplate>
                                        <asp:Label ID="_LoginName" runat="server" Text='<%# Eval("login") %>'></asp:Label>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText = "User Name">
                                <ItemTemplate>
                                        <asp:Label ID="_UserName"  runat="server" Text='<%# Eval("userName") %>'></asp:Label>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText = "Group Name">
                                <ItemTemplate>
                                        <asp:Label ID="_GroupName" runat="server" Text='<%# Eval("groupName") %>'></asp:Label>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText = "Email">
                                <ItemTemplate>
                                        <asp:Label ID="_Email"  runat="server" Text='<%# Eval("email") %>'></asp:Label>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText = "Status" Visible="false">
                                <ItemTemplate>
                                        <asp:CheckBox ID="Status_User" runat="server" Checked='<%# Bind("status") %>'></asp:CheckBox>
                                </ItemTemplate>
                           </telerik:GridTemplateColumn>
                           
                            <telerik:GridButtonColumn CommandName="Item" Visible="false" UniqueName="Disable"></telerik:GridButtonColumn>
                           <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you wish to delete?" Text="Delete" UniqueName="Delete"></telerik:GridButtonColumn>                           
                            <%--<telerik:GridCheckBoxColumn UniqueName="Status"  DataField='<%# Eval("Status") %>'></telerik:GridCheckBoxColumn>--%>
                        </Columns>  
                        
                        <Columns>
                        
                        </Columns>                                    
                    </MasterTableView>
                        
                    </telerik:RadGrid>
                    
                </ContentTemplate>
           </asp:UpdatePanel>
             
        
    </div>
    
    <div class="container_11">
        <asp:Label ID="_lblMessage" runat="server"></asp:Label>
    </div>
    
    </div>
    </form>
</body>
</html>
