<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="UnitType.aspx.cs" Inherits="UnitType" Title="" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Unit Category</title>
    <link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="_scriptManager" runat="server"></asp:ScriptManager>
        <div class="formWrap" > 
                        
               <div class="container_01">
                  <div class="container_02">
                     <label  class="form_label">
                            Unit Type Name
                     </label>
                     <div>
                        <asp:UpdatePanel ID="updateCompany" runat="server">
                            <ContentTemplate>
                                <div class="cell_label"> 
                                     <asp:TextBox ID="_txtTypeName" CssClass="form_TextBox" runat="server" MaxLength="50" ></asp:TextBox>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>                    
                     </div>
                  </div>
                </div>
           
                     
              
                 <div class="container_01">
                  <div class="assetcontainer01">
                   
                     <div>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                           <ContentTemplate>
                           <div class="assetcell">
                               <asp:Button ID="btnAdd" CssClass="form_SubmitBtn" runat="server" Text="Add"  OnClick="btnAdd_Click" />
                           </div>
                           </ContentTemplate>
                        </asp:UpdatePanel>               
                     </div>
                     <div >
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate>
                          <div class="assetcell">
                              <asp:Button ID="btnClear" CssClass="form_SubmitBtn"  runat="server" Text="Clear" OnClick="btnClear_Click" />
                          </div>
                          </ContentTemplate>
                       </asp:UpdatePanel>            
                     </div> 
                   </div>
                </div>
                
                <div class="container_01">
                    <div class="assetcontainer">
                        <div class="assetcell">
                    
                                <asp:UpdatePanel ID="uGrid" runat="server">
                                    <ContentTemplate>                
                                      <telerik:RadGrid   ID="_rgrdtype" AllowAutomaticInserts="false"  
                                            runat="server" EnableAJAX="True"  GridLines="None" Skin="Default"  
                                            AutoGenerateColumns="False" Width="400"
                                            ondeletecommand="_rgrdtype_DeleteCommand"
                                            oneditcommand="_rgrdtype_EditCommand" AllowPaging="false"
                                            onupdatecommand="_rgrdtype_UpdateCommand" onneeddatasource="_rgrdtype_NeedDataSource" 
                                            MasterTableView-EditMode="InPlace" 
                                            onitemdatabound="_rgrdtype_ItemDataBound">
                                
                                <ExportSettings>
                                    <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                                        PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                                </ExportSettings>
                                <MasterTableView DataKeyNames="typeID" CommandItemDisplay ="Top">
                                    <CommandItemSettings AddNewRecordText=""  />
                                    <ItemStyle CssClass="Text" />
                                    <Columns>
                                        <%--<telerik:GridBoundColumn DataField="typeID" HeaderText="type ID" UniqueName="typeID" Visible="False"></telerik:GridBoundColumn>--%>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltypeName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"typeName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="typeName" HeaderText="Unit Type Name" UniqueName="typeName"></telerik:GridBoundColumn>
                                        <%--<telerik:GridDropDownColumn DataField="comID" HeaderText="Company" ListTextField="ComPanyName" ListValueField="comID" UniqueName="comID" DataSourceID="SqlDataSource1" ></telerik:GridDropDownColumn>--%>
                                        <telerik:GridEditCommandColumn UniqueName="Edit"></telerik:GridEditCommandColumn>
                                        <telerik:GridButtonColumn CommandName="Delete" ConfirmText="Are you sure you want to delete this unit category?" Text="Delete" UniqueName="Delete"></telerik:GridButtonColumn>
                                    </Columns>
                                   <EditFormSettings ColumnNumber="2" CaptionFormatString="Edit Unit Type for Company with ID {0}" CaptionDataField="typeID">   
                                               <FormTableItemStyle Wrap="False"></FormTableItemStyle>  
                                               <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>  
                                               <FormMainTableStyle CellSpacing="0" CellPadding="3" Width="100%" />  
                                               <FormTableStyle GridLines="Horizontal" CellSpacing="0" CellPadding="2" CssClass="module" Height="110px" Width="100%" />  
                                               <FormTableAlternatingItemStyle Wrap="False"></FormTableAlternatingItemStyle>  
                                               <FormStyle Width="100%" BackColor="#EEF2EA"></FormStyle>  
                                               <EditColumn UpdateText="Update record" UniqueName="EditCommandColumn1" CancelText="Cancel edit">   
                                               </EditColumn>  
                                               <FormTableButtonRowStyle HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>  
                                    </EditFormSettings>  
                                    <RowIndicatorColumn Visible="False">
                                        <HeaderStyle Width="20px" />
                                        <ItemStyle CssClass="Gridtext" />
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
            
            <div class="container_01">
                <asp:UpdatePanel ID="uMsg"  runat="server">
                    <ContentTemplate>
                    <div class="form_msg_txt">
                       <asp:Label ID="_lblMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;
                   </div>
                   </ContentTemplate>
                </asp:UpdatePanel>     
              </div>     
            
        </div>
    </form>
</body>
</html>
