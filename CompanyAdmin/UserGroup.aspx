<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserGroup.aspx.cs" Inherits="CompanyAdmin_UserGroup" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Group</title>
    <link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="_scriptManager" runat="server"></asp:ScriptManager>
    <div class="formWrap">
        
        <div class="container_01">
                <div class="container_02">
                
                    <label class="form_label">
                        Enter Group Name
                    </label>
                    
                
                <div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="cell_label">
                            <asp:TextBox ID="_txtGroupName" CssClass="form_TextBox" runat="server"></asp:TextBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>                    
                </div>
                
                </div>
                
        </div>
        
        <div class="container_01">
        
            
        
        </div>    
        
       <div class="container_01">
       <div class="assetcontainer01">         
            <asp:UpdatePanel ID="uBtn" runat="server">
                <ContentTemplate>
                <div class="assetcell">
                    <asp:Button ID="btnAdd" CssClass="form_SubmitBtn" runat="server" Text="Add" OnClick="btnAdd_Click" />
                </div>
                <div class="assetcell">    
                    <asp:Button ID="btnClear" CssClass="form_SubmitBtn" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>     
       </div>
      </div>
      
      <div class="container_01" >
             <asp:UpdatePanel ID="uLbl" runat="server">
                <ContentTemplate>
                <div class="form_msg_txt">
                    <asp:Label ID="_lblMessage" runat="server"></asp:Label>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel> 
         </div>
        
        
        <div class="container_01">
        
            <div class="assetcontainer01">
                <div class="assetcell">
                
                <asp:UpdatePanel ID="uGrid" runat="server">
                    <ContentTemplate>                
                      <telerik:RadGrid ID="_rgridGroup" Width="360px" OnNeedDataSource="_rgridGroup_NeedDataSource" 
                      MasterTableView-AllowAutomaticInserts="false" 
                      OnItemCommand="_grdGroup_Update"
                      OnEditCommand="_grdgroup_Edit" 
                      OnUpdateCommand="_grdGroup_Update"  
                      runat="server" EnableAjaxSkinRendering="true"
                      GridLines="None" Skin="Default"
                      AutoGenerateColumns="False" AllowPaging="false"
                      OnDeleteCommand="_rgridGroup_DeleteCommand" 
                      OnItemDataBound="_rgridGroup_ItemDataBound">
                            <ExportSettings>
                                <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                                    PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                            </ExportSettings>
                            <MasterTableView DataKeyNames="groupID" CommandItemDisplay ="Top">
                                <CommandItemSettings AddNewRecordText="" />
                                <ItemStyle CssClass="Text" />
                                <Columns>
                                    <telerik:GridBoundColumn DataField="groupID" HeaderText="type ID" UniqueName="groupID" Visible="False"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblgrpName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"groupName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>    
                                    <telerik:GridBoundColumn DataField="groupName" HeaderText="Group Name" UniqueName="groupName"></telerik:GridBoundColumn>                                
                                    <telerik:GridEditCommandColumn UniqueName="Edit"></telerik:GridEditCommandColumn>
                                    <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Delete this user group?" Text="Delete" UniqueName="Delete"></telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings ColumnNumber="2"  CaptionFormatString="Edit details for Company with ID {0}"  
                                           CaptionDataField="groupID">   
                                           <FormTableItemStyle Wrap="False"></FormTableItemStyle>  
                                           <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>  
                                           <FormMainTableStyle CellSpacing="0" CellPadding="3" Width="100%" />  
                                           <FormTableStyle GridLines="Horizontal" CellSpacing="0" CellPadding="2" CssClass="module"  
                                               Height="110px" Width="100%" />  
                                           <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>  
                                           <FormStyle Width="100%" BackColor="#EEF2EA"></FormStyle>  
                                           <EditColumn  UpdateText="Update record" UniqueName="EditCommandColumn1" CancelText="Cancel edit">   
                                                
                                           </EditColumn>  
                                           
                                           <FormTableButtonRowStyle HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>  
                                           <FormTemplate>
                                                <asp:Label ID="lblID" runat="server" Text="this is">
                                                    
                                                </asp:Label>
                                           </FormTemplate>
                                </EditFormSettings>
                                <RowIndicatorColumn Visible="False">
                                    <HeaderStyle Width="20px" />
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Resizable="False" Visible="False">
                                    <HeaderStyle Width="20px" />
                                </ExpandCollapseColumn>
                            </MasterTableView>
                            </telerik:RadGrid>
                        </ContentTemplate>
                </asp:UpdatePanel>
                
                
                </div>
            </div>            
        </div>
         
    </div>
    
    </form>
</body>
</html>
