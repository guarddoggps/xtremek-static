<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AssinRules.ascx.cs" Inherits="CompanyAdmin_AssinRules" %>
<link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" />
<link href="../CSS/Alopek.css" rel="stylesheet" type="text/css" />
	<div class="formWrap">             
       <div class="container_01">
          <div class="container_02">
             <label>
                   Rules List
             </label>
             <div>
                 <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                 <ContentTemplate>
                 <div class="cell_label">                           
                    <asp:DropDownList CssClass="form_ddl" ID="_ddlRules" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="rulesDropDownList_SelectedIndexChanged1"></asp:DropDownList>               
                 </div>
                 </ContentTemplate>
                 </asp:UpdatePanel>                
             </div>
          </div>
        </div>
       <div class="container_01">
          <div class="assetcontainer">
             <div class="rl_formgrid">
		       <asp:UpdatePanel ID="GridUpdatePanel" runat="server">
		       <ContentTemplate>
		       <div>
		            <asp:CheckBox CssClass="formcheckbox" ID="chkAllUnit" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllUnit_Checked" />     
		       </div>
		       <div class="divcell_label">
		          Select All Unit   
		       </div>
		       <br />
               <br />
		       <div>		           
		           <asp:DataGrid ID="_grdMain" runat="server" ShowHeader="false" BorderWidth="0" OnItemCommand="_grdMain_ItemCommand" AutoGenerateColumns="false" OnItemDataBound="_grdMain_ItemDataBound">            
                            <Columns>               
                            <asp:BoundColumn DataField="typeID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <table width="100%" id="tblUnits" runat="server">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUnitGroup" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"typeName") %>'></asp:Label>
                                                <asp:CheckBox ID="chkAll"  runat="server" AutoPostBack="true" oncheckedchanged="chkAll_CheckedChanged" />
                                                <asp:Label ID="chckall01" runat="server">Check All</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color:#7b9aca; height:1px"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DataList ID="lstUnits"  RepeatColumns="7" runat="server" >
                                                    <ItemTemplate>
                                                         <asp:CheckBox ID="chkUnit"  runat="server"  />
                                                         <asp:Label ID="lblUnitID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"unitID") %>' Visible="false"></asp:Label>
                                                         <asp:Label ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"unitName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </td>
                                        </tr>
                                        
                                    </table>                        
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    </div>  
		       </ContentTemplate>
		       </asp:UpdatePanel>
		                      
             </div>
          </div>
       </div>
       <div class="container_01">
          <div class="container_02">
             <label>
                   Email Address
             </label>
             <div>
		        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                        <div class="cell_label">
                            <asp:TextBox CssClass="form_TextBox" ID="Email" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EmailRequiredFieldValidator" runat="server" 
                            ControlToValidate="Email" ErrorMessage="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="EmailRegularExpressionValidator1" 
                            runat="server" ControlToValidate="Email" 
                            ErrorMessage="Please enter valid email ID" 
                            ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"></asp:RegularExpressionValidator>
                        </div>
                        </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
       <div class="container_01">
          <div class="container_02">
             <label>
                   Geofence List
             </label>
             <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">                            
                        <asp:DropDownList CssClass="form_ddl" ID="_ddlGeofence" runat="server"></asp:DropDownList>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
       <div class="container_01">
          <div class="container_02">
             <label>
                   Email Subject
             </label>
             <div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                        <asp:TextBox CssClass="form_TextBox" ID="Subject" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="subjectRuleRequiredFieldValidator" 
                        runat="server" ControlToValidate="Subject" 
                        ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
       <div class="container_01">
          <div class="container_02">
             <label>
                   Description
             </label>
             <div>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                        <asp:TextBox CssClass="form_TextBox" ID="PName" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="DescriptionRuleRequiredFieldValidator0" 
                            runat="server" ControlToValidate="PName" 
                            ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                   
             </div>
          </div>
        </div>                                        
        <div class="form_msg_txt">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="ConfirmationFlagLabel" runat="server"></asp:Label>
                    </ContentTemplate>
            </asp:UpdatePanel>        
        </div>
       <div class="container_01">
          <div class="container_02">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                     <ContentTemplate>
                     <div class="form_msg">
                         <asp:Button CssClass="form_SubmitBtn" ID="ApplyRules" runat="server" Text="Assign Rules" 
                         onclick="ApplyRules_Click1" />
                     </div>
                     </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>        
	</div>    
    <div>
