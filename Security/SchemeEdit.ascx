<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SchemeEdit.ascx.cs" Inherits="Security_SchemeEdit" %>
    <link href="../CSS/Security.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/ajaxstyle.css" rel="stylesheet" type="text/css" />
    
    <asp:HiddenField ID="_hListValue" runat="server" />
    
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
    
    <div class="EditSchemeContainer">
          <div class="container_01">
                
               
                    
                <div class="radioBtnContainer">
                 <div class="Text"> 
                            Select Type 
                 </div> 
                    
                    <div class="container_04">
                        <div class="leftRdo">
                            <div class="rdoBtn">
                            
                                        <asp:RadioButton ID="_rdoUserGroup"  Checked="false" runat="server" AutoPostBack="true" GroupName="user" oncheckedchanged="_rdoUserGroup_CheckedChanged" />
 
                            </div>  
                            
                           <div class="rdoBtnText">
                                    User Group
                           </div>
                        </div>
                        
                       
                       <%--<div class="spacer"></div>--%>
                     
                        <div class="rightRdo">
                        
                            <div class="rdoBtn">       
                                         <asp:RadioButton ID="_rdoUser" checked="true" runat="server" AutoPostBack="true" GroupName="user" oncheckedchanged="_rdoUser_CheckedChanged" />
                                                   
                            </div>        
                        
                            <div class="rdoBtnText">
                                    User
                            </div>
                        
                        </div>
                        
                    
                    </div>                    
                </div>
          </div>
          
          <div class="container_01">
                
                <div class="container_03">
                
                     <div class="leftContainer">
                         <div class="Text">
                            Not Belonging To
                        </div>
                        <br />
                                 <asp:ListBox ID="_lstNBT"  Width="100px" runat="server"  CssClass="ListBox"  AutoPostBack="false" SelectionMode="Multiple"></asp:ListBox>   
                             
                     </div>
                     
                     <div class="width1">
                            <div>
                                <asp:Button ID="_btnRight" CssClass="form_AddBtn" OnClientClick="moveListItem(this.id,'_lstNBT','_lstBT'); return false;"   runat="server" Text=">>"  />
                            </div>    
                            <div>&nbsp;</div>
                            <div>
                                <asp:Button ID="_btnLeft"  CssClass="form_AddBtn" OnClientClick="moveListItem(this.id,'_lstBT','_lstNBT'); return false;"  runat="server" Text="<<" />
                            </div>
                     </div>
                     
                     <div class="rightContainer">
                     <div class="Text">
                            Belonging To
                     </div>
                     <br />
                                <asp:ListBox ID="_lstBT" Width="100px" CssClass="ListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                
                     </div>
               </div>
          </div>
          
          <div class="container_01 container_02">
                <div>
                    <label> 
                            Scheme Name 
                    </label>
                    
                    <div>
						<asp:TextBox ID="_txtSchemeName" CssClass="form_TextBox" runat="server"></asp:TextBox>
                    </div>
                </div>
          </div>
          
          	<div class="container_01">
           		<asp:DataGrid ID="_grdModule" CssClass="Gridtext" Width="100%" BorderWidth="1" 
                   ShowFooter="false" ShowHeader="false" runat="server" 
                   OnItemDataBound="_grdModule_DataBound" AutoGenerateColumns="false" 
                   OnUpdateCommand="_grdModule_UpdateCommand">
                   <ItemStyle CssClass="Gridtext" />

                   	<Columns>                    
                        <asp:BoundColumn DataField="ID" Visible="false"></asp:BoundColumn>

                      	<asp:TemplateColumn>
                         	<ItemTemplate>
                            	<div class="gridHeader" id="_moduleHeader" runat="server">
                                	<asp:CheckBox ID="_chkAll" runat="server" AutoPostBack="true" OnClick="checkAll()"/> <%# Eval("moduleName") %>  
                               	</div>

                               	<div>
                               		<asp:DataList ID="_dlstPages" runat="server" OnItemDataBound="_dlstPages_ItemDataBound" RepeatColumns="1">
		                               	<ItemStyle CssClass="Gridtext" />
		                              	<ItemTemplate>
		                               		<div class="mainAccesibility">
		                               			<div class="leftAccess">
				                       				<asp:Label ID="_lblStatus" runat="server" Text='<%# Eval("fullAccess") %>' Visible="false"></asp:Label>
				                      				<asp:CheckBox ID="_chkPage" runat="server" />
				                                    <asp:Label ID="_lblPageID" runat="server" Visible="false" Text='<%# Eval("formID") %>'></asp:Label>
				                                    <asp:Label ID="_lblPageName" runat="server" Text='<%# Eval("formName") %>'></asp:Label>
				                                </div>
		                                        <div class="rightAccess">
				                                	<asp:Label ID="_lblInsertStatus" runat="server" Text='<%#Eval("insert") %>' Visible="false"></asp:Label>
				                                   	<asp:CheckBox ID="_chkInsert" runat="server" />
				                                    <asp:Label ID="_lblInsert" runat="server" Text="Insert"></asp:Label>                                                
				                                       
				                                   	<asp:Label ID="_lblViewStatus" runat="server" Text='<%#Eval("view") %>' Visible="false"></asp:Label>
				                                    <asp:CheckBox ID="_chkView" runat="server" />
				                                    <asp:Label ID="_lblView" runat="server" Text="View"></asp:Label>                                                
				                                        
				                                    <asp:Label ID="_lblEditStatus" runat="server" Text='<%#Eval("Edit") %>' Visible="false"></asp:Label>
				                                    <asp:CheckBox ID="_chkEdit" runat="server" />
				                                    <asp:Label ID="_lblEdit" runat="server" Text="Edit"></asp:Label>                                                
				                                       
				                                  	<asp:Label ID="_lblDeleteStatus" runat="server" Text='<%#Eval("delete") %>' Visible="false"></asp:Label>
				                                 	<asp:CheckBox ID="_chkDelete" runat="server" />
				                                 	<asp:Label ID="_lblDelete" runat="server" Text="Delete"></asp:Label>      
				                                </div>   
		                           			</div>
		                                                                
		                            	</ItemTemplate>
                              		</asp:DataList>
                          		</div>
                       		</ItemTemplate>
                  		</asp:TemplateColumn>
            		</Columns>
         		</asp:DataGrid>            
          	</div>
          
          	<div class="container_01 container_02">
                <div>
                        <label>
                                Set as Default Scheme
                        </label>
                        <div>
                            <asp:CheckBox ID="_chkDefaultScheme" runat="server" />
                        </div>
                </div>
          	</div>
          	<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtSchemeName"
                             ErrorMessage="Scheme Name is required." ToolTip="Scheme Name is required.">Scheme Name is required.</asp:RequiredFieldValidator>
          	<div class="container_01">
                    
                    <div class="leftContainer">
                        <asp:Button ID="_btnSave" CssClass="form_SubmitBtn" runat="server" Text="Save" CommandName="Update"  />
                    </div>
                    
                    <div class="rightContainer">
                        <asp:button id="btnCancel"  CssClass="form_SubmitBtn" text="Cancel" runat="server" causesvalidation="False" commandname="Cancel"></asp:button>
                    </div>
                    
                               
          	</div>
          
          <div>
                        <asp:Label ID="_lblMessage" runat="server"></asp:Label>
             
          </div>
              <%-- <asp:button id="btnCancel" text="Cancel" runat="server" causesvalidation="False" commandname="Cancel"></asp:button>  --%>   
    </div>
