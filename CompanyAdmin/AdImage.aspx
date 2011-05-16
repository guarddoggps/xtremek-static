<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdImage.aspx.cs" Inherits="CompanyAdmin_AdImage" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Advertising Image</title>
    <script type="text/javascript">
        
    </script>
    
    <link href="../CSS/TwoColForms.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
    
    <form id="form1" runat="server">
        <asp:ScriptManager ID="GridScriptManager" runat="server"> </asp:ScriptManager>
    <div class="formWrap">
        
       
         <div class="container_01">
            <div class="container_02">
                <div>
                   <label>
                        Add Image
                   </label>
                </div>
                          
                     
                <div >
                    <asp:Image ID="_imgPicture" runat="server" Visible="false" Width="160" Height="120" />
                
                    <asp:FileUpload ID="FileInput" runat="server" CssClass="form_TextBox" />
                    
                </div>
            
         
            </div>
         </div>
         
         <div class="container_01">
            <div class="container_02">
                   
                   <label>
                        Enter URL:
                   </label>
                                      
            
                    <div >
                            <asp:TextBox ID="_txtURL" runat="server" CssClass="form_TextBox"></asp:TextBox>
                    </div>
            </div>
         </div>
         <div class="container_01">
            <div class="container_02">
                <div>
                    <asp:CheckBox ID="_chkIsActive" runat="server" Text="Active Image" TextAlign="Left" />
                    
                </div>
            </div>
         </div>
         
          <div class="container_01">
            <div class="container_02">            
                <asp:Button ID="_btnUpload" CssClass="form_SubmitBtn" runat="server" Text="Save" 
                    onclick="_btnUpload_Click" />
            </div>
            
         </div>  
         
         <div class="container_01">
                
         <asp:UpdatePanel ID="updateGrid" runat="server">
            <ContentTemplate>            
            
              <telerik:RadGrid ID="_rgrdAdImage"  runat="server" Skin="Office2007" 
                  AllowPaging="true" PageSize="10"
                           GridLines="None" 
                  MasterTableView-CommandItemDisplay="TopAndBottom" AllowSorting="True"  AutoGenerateColumns="False"  
                           Width="99%" OnNeedDataSource="_rgrdAdImage_NeedDataSource" 
                  OnDeleteCommand="_rgrdAdImage_DeleteCommand"  
                  OnUpdateCommand="_rgrdAdImage_UpdateCommand" EnableAJAX="True" 
                  onitemdatabound="_rgrdAdImage_ItemDataBound"  >  
                           <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>  
                            
                           <MasterTableView DataKeyNames="ID" EditMode="InPlace" GridLines="None"  Width="100%" CommandItemDisplay ="Top" >  
                                <CommandItemSettings AddNewRecordText="" AddNewRecordImageUrl=""/>
                               
                               <Columns>  
                                    
                                   <telerik:GridBoundColumn DataField="ID" HeaderText="comID" UniqueName="comID" Visible="false" ReadOnly="True">   
                                   </telerik:GridBoundColumn>
                                    
                                   
                                   <telerik:GridBoundColumn DataField="ImageUrl"  HeaderText="URL" UniqueName="ImageUrl">   
                                   </telerik:GridBoundColumn> 
                                   <telerik:GridTemplateColumn HeaderText="Image">
                                    <ItemTemplate>
                                        <asp:Image ID="AddImage" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem,"Image") %>' />
                                    </ItemTemplate>
                                   </telerik:GridTemplateColumn> 
                                   <telerik:GridTemplateColumn Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="_lblImageName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ImageName") %>'></asp:Label>
                                    </ItemTemplate>
                                  </telerik:GridTemplateColumn>     
                                  <telerik:GridCheckBoxColumn DataField="isActive"  HeaderText="Status" UniqueName="isActive">
                                   </telerik:GridCheckBoxColumn>
                                   
                                   
                                   
                                   <telerik:GridEditCommandColumn>  
                                   </telerik:GridEditCommandColumn> 
                                   
                                   <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Do you wish to delete?"  Text="Delete" UniqueName="Delete">   
                                   </telerik:GridButtonColumn>  
                               </Columns>  
                               
                               <EditFormSettings ColumnNumber="2" CaptionFormatString="Edit details for Company with ID {0}"  
                                   CaptionDataField="ID"> 
                                    <FormTemplate>
                                        
                                    </FormTemplate>
                                   <FormTableItemStyle  Wrap="False"></FormTableItemStyle>  
                                   <FormCaptionStyle  CssClass="EditFormHeader"></FormCaptionStyle>  
                                   <FormMainTableStyle  CellSpacing="0" CellPadding="3" Width="100%" />  
                                   <FormTableStyle  GridLines="Horizontal" CellSpacing="0" CellPadding="2" CssClass="module"  
                                       Height="110px" Width="100%" />  
                                   <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>  
                                   <FormStyle Width="100%"  BackColor="#EEF2EA"></FormStyle>  
                                   <EditColumn UpdateText="Update record" InsertText="" UniqueName="EditCommandColumn1" CancelText="Cancel edit">   
                                   </EditColumn>  
                                   <FormTableButtonRowStyle   HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>  
                               </EditFormSettings>  
                               <ExpandCollapseColumn Visible="False">   
                                   <HeaderStyle Width="19px"></HeaderStyle>  
                               </ExpandCollapseColumn>  
                               <RowIndicatorColumn Visible="False">   
                                   <HeaderStyle Width="20px" />  
                               </RowIndicatorColumn>  
                           </MasterTableView>  
               </telerik:RadGrid> 
          
             </ContentTemplate>
          </asp:UpdatePanel>  
          
         </div>
         
        
         
         <div>
                <asp:Label ID="_lblMessage" runat="server"></asp:Label>
         </div>
         
    </div>
    </form>
</body>
</html>
