<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucSpeeding.ascx.cs" Inherits="CompanyAdmin_ucSpeeding" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" />
<div class="formWrap">             
       <div class="container_01">
          <div class="container_02">
             <label>
                    Rules For Speeding
             </label>             
          </div>
        </div>
      
       <div class="container_01">
          <div class="container_02">
             <label>
                    Values
             </label>
             <div>
                 <asp:UpdatePanel ID="updateValues" runat="server">
                 <ContentTemplate>
                 <div class="cell_label">                    
                       <asp:TextBox ID="_txtValue" CssClass="form_TextBox" runat="server"></asp:TextBox>
                       
                 </div>
                 </ContentTemplate>
                 </asp:UpdatePanel>                
             </div>
          </div>
        </div>
        <div class="form_msg_txt">
            <asp:UpdatePanel ID="MsgLabelUpdatePanel" runat="server">
            <ContentTemplate>            
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </ContentTemplate>
            </asp:UpdatePanel>                    
        </div>
       <div class="container_01">
          <div class="container_02">
             <div>
                <asp:UpdatePanel ID="SaveButton" runat="server">
                <ContentTemplate>
                <div class="form_msg">
                 <asp:Button ID="btnSave" CssClass="form_SubmitBtn" runat="server" OnClick="btnSave_Click" Text="Save"></asp:Button>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
        <div class="container_01">
           <div class="container_02">
              <div class="rl_text">  
                Current Rules :
              </div>  
           </div> 
        </div>
       <div class="container_01">
          <div class="assetcontainer">
           <div class="assetcell">
            <asp:UpdatePanel ID="gridUpdatePanel" runat="server">
            <ContentTemplate>
            <telerik:RadGrid ID="_rgriDRules" runat="server" EnableAJAX="True" GridLines="None" Skin="Default2006" Width="380px" AutoGenerateColumns="False" OnNeedDataSource="_rgriDRules_NeedDataSource" OnDeleteCommand="_rgriDRules_DeleteCommand" OnItemDataBound="_rgriDRules_ItemDataBound"  >
                <ExportSettings>
                    <Pdf PageBottomMargin="" PageFooterMargin="" PageHeaderMargin="" PageHeight="11in"
                        PageLeftMargin="" PageRightMargin="" PageTopMargin="" PageWidth="8.5in" />
                </ExportSettings>
                <MasterTableView DataKeyNames="RulesID" CommandItemDisplay ="Top">
                <CommandItemSettings AddNewRecordText="" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="RulesID" HeaderText="RulesID" UniqueName="RulesID" Visible="False"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Rules" HeaderText="Rules" UniqueName="Rules"></telerik:GridBoundColumn>                        
                    </Columns>
                    <RowIndicatorColumn Visible="False">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Resizable="False" Visible="False">
                        <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                </MasterTableView>
            </telerik:RadGrid> 
            </ContentTemplate>
            </asp:UpdatePanel> <br />
           </div>
          </div>
        </div>
        <div>
        </div>
      </div>   