<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnitManagement.aspx.cs" Inherits="CompanyAdmin_UnitManagement" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unit Management</title>
    <link href="../CSS/TwoColForms.css" rel="stylesheet" type="text/css" />
    <script src="../Js/DateTime.js" type="text/javascript"></script>
    <script src="../Js/NumericValidation.js" type="text/javascript"></script>    
    
    <script type="text/javascript">
      
      function changeImg(control)
        {
           var t= document.getElementById("imgIcon");
           t.src=control.src;
           
           var cntl1=document.getElementById("imgSource");
            cntl1.value=control.src;          
        }
        
        var isTimeSelected = false;
        function DateSelected(sender, args)
        {
            if (!isTimeSelected)
                sender.GetTimeView().SetTime(8, 0, 0);
                
            isTimeSelected = false;
        }
        function ClientTimeSelected(sender, args)
        {
            isTimeSelected = true;
        }
       
           
        function get2Day()
        {
                var dateCntl1=document.getElementById("txtUnitPerchaseDate_wrapper");
                var dateCntl2=document.getElementById("txtDevicePerchaseDate_wrapper");
                var DateTime=getDate("","","");
                var DateTime1=getDate("","","");
                var dt1=new Date(DateTime);
                var dt2=new Date(DateTime1);
                dateCntl1.setAttribute("SetDate");
                dateCntl2.setAttribute("SetDate");
                dateCntl1.SetDate(dt1);
                dateCntl2.SetDate(dt2); 
         }       
      
   
      function btnMoveRight_Click(leftbox,rightbox)
        {
      
            var leftListbox = document.getElementById(leftbox);
            var rightListbox = document.getElementById(rightbox);

            if ((leftListbox != null) && (rightListbox != null)) 
            { 
                if(leftListbox.length < 1) 
                {
                    alert('There are no items in the source ListBox');
                    return false;
                }
                if(leftListbox.options.selectedIndex == -1) // when no Item is selected the index will be -1
                {
                    alert('Please select an Item to move');
                    return false;
                }
                while ( leftListbox.options.selectedIndex >= 0 ) 
                 { 
                    var newOption = new Option(); // Create a new instance of ListItem 
                    newOption.text = leftListbox.options[leftListbox.options.selectedIndex].text; 
                    newOption.value = leftListbox.options[leftListbox.options.selectedIndex].value; 
                    rightListbox.options[rightListbox.length] = newOption; //Append the item in Target Listbox
                    leftListbox.remove(leftListbox.options.selectedIndex); //Remove the item from Source Listbox 
                } 
            }
                _setHiddenValue();
               return false; 
               
          }
        function _setHiddenValue()
        {
            var listString ="";
            var selectedList = document.getElementById("lstBT");
            
            for(var i = 0 ; i < selectedList.options.length ; i++)
            {
                listString += selectedList.options[i].value + " ; ";
            }
            
            document.getElementById("_hListValue").value = listString ;
        }
    </script> 
</head>

<body>
	<form id="form1" runat="server">
		<asp:HiddenField ID="_hListValue" runat="server" />
      	<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <div class="formWrap">
          	<div class="container_01">
           		<div class="container_02">
           			<div>
                		<label>Select Unit</label>
           			</div>
           			
           			<div>   
           			
              <asp:UpdatePanel ID="UpdatePanel6" runat="server">
               <ContentTemplate>  
                  <div class="cell_label">                                      
                      <asp:DropDownList ID="cmbUnitName" runat="server" CssClass="form_ddl" AutoPostBack="true" onselectedindexchanged="cmbUnitName_SelectedIndexChanged"></asp:DropDownList>
                      
                  </div>
               </ContentTemplate>
               </asp:UpdatePanel> 
               
          </div> 
 
           
         
                                                                                                                                                                                           
       </div>
    </div>
                     
         <div class="container_01">
           <div class="container_02">
           
           <div>   
                   
                 
               <asp:UpdatePanel ID="UpdatePanel24" runat="server">
               <ContentTemplate>         
                 <label id="lblUnitID" runat="server">Unit ID</label>
               </ContentTemplate>
               </asp:UpdatePanel> 
               
               <asp:UpdatePanel ID="update" runat="server">
               <ContentTemplate>  
                  <div class="cell_label">                                      
                      <%--<asp:DropDownList ID="cmbUnitName" runat="server" CssClass="form_ddl" AutoPostBack="true" onselectedindexchanged="cmbUnitName_SelectedIndexChanged"></asp:DropDownList>--%>
                      <asp:TextBox ID="txtDevice" runat="server" CssClass="form_TextBox" onkeypress=" return(allownumbers(event))"></asp:TextBox>
                  </div>
               </ContentTemplate>
               </asp:UpdatePanel> 
               
          </div> 
 
           
          <div class="gap">
           </div>  
                <div>
                <label>
                    Unit Name 
                </label>
                
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                      <ContentTemplate>
                        <div class="cell_label">
                            <asp:TextBox ID="txtName"  runat="server" CssClass="form_TextBox" ></asp:TextBox>
                                 
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtName" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel> 
           
                    
          </div> 
                                                                                                                                                                                           
       </div>
    </div>   
         
     <div class="container_01">
            <div class="container_02">
             
                <div> 
                
                   <label>
                        Unit Catagory 
                   </label>              
                   <asp:UpdatePanel ID="updateVehicle" runat="server">
                       <ContentTemplate>
                       <div class="cell_label">                        
                           <asp:DropDownList ID="cmbVehicleCat" runat="server" CssClass="form_ddl" AutoPostBack="false"></asp:DropDownList>                    
                       </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>                        
                </div>
                
                <div class="gap">
           </div>   
                
                <div>
                   <label>Maintainence Pattern</label>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                        <div class="cell_label">
                            <asp:DropDownList ID="cmbPattern" runat="server" CssClass="form_ddl" AutoPostBack="false"></asp:DropDownList>                    
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>                                                                                                                                                                                               
         </div>       
         
         
        <div class="container_01">
            <div class="container_02">
            
            <div>
                <label>
                    License Plate
                </label>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                    <div class="cell_label">                        
                        <asp:TextBox ID="txtLicense" CssClass="form_TextBox" runat="server"></asp:TextBox>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div class="gap">
           </div>
             
              <div>
            
                <label>
                    Other Info
                </label>
                     <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                        <ContentTemplate>
                <div class="cell_label">
                        <asp:TextBox ID="txtOtherInfo" CssClass="form_TextBox" runat="server"></asp:TextBox>
                </div>
                  </ContentTemplate>
                    </asp:UpdatePanel>
              </div>

            </div>
                                                                                                                                                                                                           
         </div>                                                  
         
        	<div runat="server" id="groupHeader" class="form_Header">
                <div class="text_header">
                    Groups           
                </div>
          	</div>

            <div style="padding-bottom: 10px;" class="container_01">
                    <div runat="server" id="lblNBT" class="list_label">
                        Not Belonging to
                    </div>
                    <div class="formlistbox_a">
                        <asp:UpdatePanel ID="addlstnbt" runat="server">
                                <ContentTemplate> 
                                   <asp:ListBox ID="lstNBT" SelectionMode="Multiple"   runat="server" AutoPostBack ="false"  CssClass="ListBox"></asp:ListBox>                                
                                </ContentTemplate>
                      </asp:UpdatePanel>
                    </div>
                    <div class="formlistbox_b">
                        <asp:UpdatePanel ID="id01" runat="server">
                            <ContentTemplate>
		                    	<asp:Button ID="_btnRight" class="form_AddBtn" runat="server" Text=">>" OnClientClick="btnMoveRight_Click('lstNBT','lstBT'); return false;"/>     
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:UpdatePanel ID="id02" runat="server">
                            <ContentTemplate>
		                    	<asp:Button ID="_btnLeft" class="form_AddBtn" runat="server" Text="<<" OnClientClick="btnMoveRight_Click('lstBT','lstNBT'); return false;"/>                                     
                            </ContentTemplate>
                        </asp:UpdatePanel>               
                     </div>                       
                    <div>
                        <div runat="server" id="lblBT" class="list_label">
                            Belonging to
                        </div>
                        <div class="formlistbox_a">
                            <asp:UpdatePanel ID="addlstbt" runat="server">
                                <ContentTemplate>                             
                                        <asp:ListBox ID="lstBT" SelectionMode="Multiple"   runat="server" AutoPostBack ="false" CssClass="ListBox"></asp:ListBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
      	</div>

             <div class="form_Header">
                <div class="text_header">
                    Unit Icons           
                </div>
             </div>
             <div class="container_01">
                <div class="iconcontainer">
                    <div class="iconformLabel">
                        <label>Click on any image to select an Icon</label>
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                <div class="iconimagecontainersingle">   
                                    <img id="imgIcon" src="../Icon/Default.png" width="50" height="50"   runat="server"/>                               
                                    <input type="hidden" id="imgSource" runat="server" />
                                </div>
                                </ContentTemplate>
                           </asp:UpdatePanel>                        
                        </div>
                        <div>
                            <label>Current Icon</label>
                        </div>
                        
                    </div>
                    <div>
                        <asp:UpdatePanel ID="uImg" runat="server">
                            <ContentTemplate>
                            <div class="iconcelllabel">
                                 <asp:DataList ID="dlstIcon" RepeatColumns="8" runat="server" >
                                    <SeparatorTemplate>
                                    </SeparatorTemplate>
                                    <ItemTemplate>
                                         <img id="Img1"  src='<%# DataBinder.Eval(Container.DataItem,"icon") %>' runat="server" width="50" height="50" onclick="changeImg(this);" />
                                    </ItemTemplate>
                                </asp:DataList>
                           </div>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                  </div>
             </div>
            <div class="container_01">
                  <div class="container_02">
                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                <ContentTemplate>
                    <div class="buttonformLabel">
                        <!--<input type="submit" runat="server" class="form_SubmitBtn" value="Save" id="Submit" />-->
                        <asp:Button ID="btnSubmit" CssClass="form_SubmitBtn" runat="server" Text = "Save" onclick="btnSubmit_Click"  OnClientClick="get2Day()"/>
                  </div>
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                    
                    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                    <ContentTemplate>
                   <div class="buttonformmsg">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                 </div>
            </div> 
            </div>    
    </form>
</body>
</html>
