<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditRules.ascx.cs" Inherits="CompanyAdmin_EditRules" %>
<link href="CSS/Webforms.css" rel="stylesheet" type="text/css" />
    <div class="formWrap">       
       <div class="container_01">
          <div class="container_02">
             <label>
                   Rules List
             </label>
             <div>
                <asp:UpdatePanel ID="RulesUpdatePanel" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                         <asp:DropDownList CssClass="form_ddl" ID="_ddlRules" runat="server" AutoPostBack="True" 
                         onselectedindexchanged="_ddlRules_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
       </div>
       <div class="rl_container_01">
          <div class="container_02">
             <label>
                   Units
             </label>
             <div>
                <asp:UpdatePanel ID="_unitsddl" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                        <asp:DropDownList CssClass="form_ddl" ID="_ddlUnits" runat="server" 
                        AutoPostBack="True" onselectedindexchanged="_ddlUnits_SelectedIndexChanged" 
                             >
                        </asp:DropDownList>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
       <%--<div class="container_01">
          <div class="form_msg01">
            <asp:UpdatePanel ID="CancelUnitUpdatePanel" runat="server">
                <ContentTemplate>
                <div >
                    <asp:DataList ID="CancelUnitDataList" RepeatColumns="4" runat="server">
                        <ItemTemplate>
                            <asp:CheckBox ID="UnitCheckBox"  runat="server" Checked="true" TextAlign="Right" />
                            <asp:Label ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"unitName") %>'></asp:Label>
                            <asp:Label ID="lblUnitID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"unitID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>
          </div>
        </div>--%>
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
                   Email Address
             </label>
             <div>
                <asp:UpdatePanel ID="emailUpdatePanel" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                        <asp:TextBox CssClass="form_TextBox" ID="EmailTextBox" runat="server" MaxLength="100"></asp:TextBox>
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
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                        <asp:TextBox CssClass="form_TextBox" ID="SubjectTextBox" runat="server"></asp:TextBox>
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
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">
                        <asp:TextBox CssClass="form_TextBox" ID="DescriptionTextBox" runat="server"></asp:TextBox>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
       <div class="container_01">
          <div class="container_02">
             <label>
                   Status
             </label>
             <div class="cell_label">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                    <div>
                        <asp:CheckBox CssClass="formcheckbox" ID="isActiveCheckBox" runat="server"/>
                    </div>
                    <div class="divcell_label">
                        Is Active
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>
        <div class="form_msg_txt">
            <asp:UpdatePanel ID="MessageUpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Label ID="ConfirmationFlageLabel" runat="server" Text=""></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>            
        </div>
       <div class="container_01">
          <div class="container_02">
             <div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                    <div class="form_msg">
                        <asp:Button CssClass="form_SubmitBtn" ID="EditButton" runat="server" Text="Update Rules" 
                        onclick="EditButton_Click" />
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>                
             </div>
          </div>
        </div>                                                                      
    </div>
