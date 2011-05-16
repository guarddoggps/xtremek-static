<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="CompanyAdmin_UserManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Management</title>
    <link href="../CSS/Webforms.css" rel="stylesheet" type="text/css" /> 
    <link href="../CSS/ajaxstyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
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


            function GetRadWindow()
            {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;//IE (and Moz az well)
                return oWindow;
            }

            function CloseOnReload()
            {
                GetRadWindow().Close();
            }
        function _setHiddenValue()
        {
            var listString ="";
            var selectedList = document.getElementById("lstBT");
            
            for(var i = 0 ; i< selectedList.options.length ; i++)
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" >
        </asp:ScriptManager>
                         
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
        
        <div class="formWrap">
        
              
	 	<div class="form_Header">
		 	<div class="text_header">User Information</div>       
	   	</div>

		<div id="_divUser" runat="server" class="container_01">
                <label class="form_label">Select User</label>

		        <div id="_divUser444">
		        	<asp:UpdatePanel ID="UpdatePanel4" runat="server">
		            	<ContentTemplate>
		                	<div class="cell_label"> 
		                    	<asp:DropDownList CssClass="form_ddl" ID="_ddlUser" runat="server"  OnSelectedIndexChanged="_ddlUser_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
		                   	</div>
		              	</ContentTemplate>
		          	</asp:UpdatePanel>                    
		     	</div>

		</div>            
		        
		<div id="divko" class="container_01">
			<div class="container_02">
		    	<label class="form_label">Login Name</label>

		     	<div>
		        	<asp:UpdatePanel ID="UpdatePanel2" runat="server">
		            	<ContentTemplate>
		                	<div class="cell_label"> 
		                    	<asp:TextBox ID="_txtLoginName" runat="server" MaxLength="15" CssClass="form_TextBox"></asp:TextBox>
		                    	<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_txtLoginName"
		                         	 ErrorMessage="Login is required." ToolTip="User Name is required.">*</asp:RequiredFieldValidator>
		                	</div>
		               	</ContentTemplate>
		          	</asp:UpdatePanel>                    
		       	</div>

		  	</div>
	   	</div> 
		           
	   	<div class="container_01">
			<div class="container_02">
		     	<label class="form_label">Group Name</label>

		    	<div>
		        	<asp:UpdatePanel ID="updateGroup" runat="server">
		          		<ContentTemplate>
		                 	<div class="cell_label"> 
		                   		<asp:DropDownList CssClass="form_ddl" ID="_ddlGroup" runat="server"></asp:DropDownList>
		                  	</div>
		             	</ContentTemplate>
		         	</asp:UpdatePanel>                    
		      	</div>

		   	</div>
	   	</div>
		 
		<div class="container_01">
			<div class="container_02">
		    	<label class="form_label">Full Name</label>

		   		<div>
					<asp:UpdatePanel ID="UpdatePanel7" runat="server">
		      			<ContentTemplate>
		           			<div class="cell_label">
		            			<asp:TextBox ID="_txtFullName" runat="server" MaxLength="80" CssClass="form_TextBox"></asp:TextBox>
		               			<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="_txtFullName"
		                    	 	 ErrorMessage="User Name is required." ToolTip="User Name is required.">*</asp:RequiredFieldValidator>                     
		         			</div>
		      			</ContentTemplate>
		      		</asp:UpdatePanel>
		 		</div>

		   	</div>
	 	</div>
		        
	  	<div class="container_01">
			<div class="container_02">
		     	<label class="form_label">Password</label>

		        <div>
		        	<asp:UpdatePanel ID="UpdatePass" runat="server">
		            	<ContentTemplate>
		                 	<div class="cell_label">
				                <asp:TextBox ID="_txtPassword"  runat="server" TextMode="Password" MaxLength="15" CssClass="form_TextBox"></asp:TextBox>
				                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="_txtPassword"
		                    		 ErrorMessage="Password is required." ToolTip="Password is required.">*</asp:RequiredFieldValidator>
		                 	</div>
		               	</ContentTemplate>
		          	</asp:UpdatePanel>
		      	</div>

		   	</div>
	  	</div>

	   	<div class="container_01">
			<div class="container_02">
		     	<label class="form_label">Confirm Password</label>

		       	<div>
		       		<asp:UpdatePanel ID="UpdateConfPass" runat="server">
		            	<ContentTemplate>
		                	<div class="cell_label">
		                    	<asp:TextBox ID="_txtConfirmPassword" runat="server" TextMode="Password" MaxLength="15" CssClass="form_TextBox"></asp:TextBox>
		                    	<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="_txtConfirmPassword"
		                    		 ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required.">*</asp:RequiredFieldValidator>
		                	</div>
		             	</ContentTemplate>
		          	</asp:UpdatePanel>
		       	</div>

			</div>
	 	</div>

	  	<div class="container_01">
			<div class="container_02">
		    	<label class="form_label">Email</label>

		      	<div>
		       		<asp:UpdatePanel ID="UpdatePanel10" runat="server">
		          		<ContentTemplate>
		                	<div class="cell_label">
		                 		<asp:TextBox ID="_txtEmail" runat="server" MaxLength="80" CssClass="form_TextBox"></asp:TextBox>
		                	</div>
		               	</ContentTemplate>
		        	</asp:UpdatePanel>
		      	</div>

		   	</div>
	  	</div>

	   	<div class="container_01">
			<div class="container_02">
		      	<label class="form_label">Security Question</label>
		        
				<div>
		        	<asp:UpdatePanel ID="UpdatePanel11" runat="server">
		            	<ContentTemplate>
		                	<div class="cell_label">
		                    	<asp:DropDownList ID="_ddlSecurityQuestion" runat="server" CssClass="form_ddl2" AutoPostBack="false"></asp:DropDownList>
		                	</div>
		               	</ContentTemplate>
		           	</asp:UpdatePanel>
		      	</div>

		   	</div>
	   	</div>

		<div class="container_01">
		 	<div class="container_02">
		     	<label class="form_label">Security Answer</label>

		       	<div>
		         	<asp:UpdatePanel ID="UpdatePanel12" runat="server">
		           		<ContentTemplate>
		                	<div class="cell_label">
		                    	<asp:TextBox ID="_txtSecurityA"  runat="server" MaxLength="200" CssClass="form_TextBox"></asp:TextBox>
		                    	<asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="_txtSecurityA"
		                    		 ErrorMessage="Security answer is required." ToolTip="Security answer is required.">*</asp:RequiredFieldValidator>
		                	</div>
		                </ContentTemplate>
		      		</asp:UpdatePanel>
		     	</div>

		 	</div>
		</div>
		        
	  	<div class="container_01">
		   	<div class="container_02">
		      	<label class="form_label">Security Scheme</label>
		         
		       	<div>
					<asp:UpdatePanel ID="_schm" runat="server">
		      			<ContentTemplate>
		            		<div class="cell_label">
		                		<asp:DropDownList ID="_ddlSecurityScheme" runat="server" CssClass="form_ddl"></asp:DropDownList>
		             		</div>
		                    
		         		</ContentTemplate>
		     		</asp:UpdatePanel>
		   		</div> 
	  
		   	</div>            
	 	</div>

		<div class="container_01">
	  		<div class="container_02">
		     	<label class="form_label">User Status</label>  
		          
		        <div class="cell_label">
		       		<asp:UpdatePanel ID="UpdatePanel14" runat="server">
		          		<ContentTemplate>
		                 	<div>
		                    	<asp:CheckBox ID="_chkIsActive" runat="server" CssClass="formcheckbox"/>
		                 	</div>
		                  	<div class="divcell_label">Is Active</div>
		       			</ContentTemplate>
		         	</asp:UpdatePanel>
		     	</div>       
		  
			</div>  
	  	</div>      
		     
		<!-- Unit Group -->  
		<div runat="server" id="unitGroupContainer">                                                
		 	<div class="form_Header">
			 	<div class="text_header">Unit Group</div>       
		   	</div>

		  	<div class="container_01">
				<div class="container_02">
					<label class="form_label">Select Type</label>

				    <div class="cell_label" >
				  		<asp:UpdatePanel ID="UpdatePanel5" runat="server">
				         	<ContentTemplate>
				       			<div class="radiobutton">
				                	<asp:RadioButton GroupName="Unit" AutoPostBack="true" runat="server" 
				                         ID="_rdoUnitGroup" Checked="true" oncheckedchanged="_rdoUnitGroup_CheckedChanged" />
				              	</div>
				         	</ContentTemplate>
				     	</asp:UpdatePanel> 

			   			<div class="rl_cell_label" >Unit Groups</div>

				      	<asp:UpdatePanel ID="UpdatePanel15" runat="server">
				         	<ContentTemplate>
				        		<div class="radiobutton">
				                	<asp:RadioButton ID="_rdoUnits" AutoPostBack="true" runat="server" 
				                         GroupName="Unit" oncheckedchanged="_rdoUnits_CheckedChanged" />
				             	</div>
				           	</ContentTemplate>
				      	</asp:UpdatePanel>

				  		<div class="rl_cell_label">Units List</div> 

			  		</div>                  
			 	</div>
		 	</div>

			<div class="container_01">
				<div class="container_02">
					<label class="form_label">Not Belonging To</label>

			   		<div class="text_01">Belonging To</div>
			   	</div>
			</div>

		   	<div class="container_01">
				<div>
					<div class="formlistbox_a">
				    	<asp:UpdatePanel ID="UpdatePanelNList" runat="server">
				        	<ContentTemplate>
				           		<asp:ListBox ID="lstNBT" CssClass="ListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
				          	</ContentTemplate>
				      	</asp:UpdatePanel>                    
				  	</div>   

			  		<div class="formlistbox_b">
				    	<asp:UpdatePanel ID="UpdatePanel3" runat="server">
				      		<ContentTemplate>
				            	<asp:Button ID="_btnRight" class="form_AddBtn" Text=">>" runat="server" OnClientClick="btnMoveRight_Click('lstNBT','lstBT'); return false;"/> <br />
				                <asp:Button ID="_btnLeft" class="form_AddBtn" runat="server" Text="<<" OnClientClick="btnMoveRight_Click('lstBT','lstNBT'); return false;"/>                                     
				         	</ContentTemplate>
				       	</asp:UpdatePanel>                 
				 	</div>

				  	<div class="formlistbox_c">
				   		<asp:UpdatePanel ID="addList" runat="server">
				    		<ContentTemplate>
				              	<asp:ListBox ID="lstBT" CssClass="ListBox" runat="server" SelectionMode="Multiple"></asp:ListBox>
				   		</ContentTemplate>
				    	</asp:UpdatePanel>                      
			   		</div>

		   		</div>               
		  	</div>
		</div>

	  	<div class="form_msg_txt">
		  	<asp:UpdatePanel ID="UpdatePanel13" runat="server">
		     	<ContentTemplate>
		     		<asp:Label ID="_lblMessage" runat="server"></asp:Label>
		     	</ContentTemplate>
		   	</asp:UpdatePanel> 
	   	</div>

	   	<div class="form_msg_txt">
			<asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="_txtPassword"
		       	 ControlToValidate="_txtConfirmPassword" Display="Dynamic" 
		         ErrorMessage="The Password and Confirmation Password must match."></asp:CompareValidator>            
	   	</div>

	  	<div class="form_msg_txt" style="color: red">
			<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
	  	</div> 

	  	<div class="container_01">
		 	<div class="container_02">
		   		<div>
		       		<asp:UpdatePanel ID="panelcreatebutn" runat="server">
		            	<ContentTemplate>
		                 	<div class="assetcell">
		                    <asp:Button ID="_btnSave" CssClass="form_SubmitBtn" runat="server" Text="Save" 
		                         onclick="_btnSave_Click" OnClientClick="_setHiddenValue();" Height="26px"/>
		             		</div>
		          		</ContentTemplate>
		     		</asp:UpdatePanel>                                       
		  		</div>  
		  
		    	<div>
		        	<asp:UpdatePanel ID="panelcancelbutn" runat="server">
		            	<ContentTemplate>
		                	<div class="assetcell">
		                    	<asp:Button ID="_btnCancel" CssClass="form_SubmitBtn" runat="server" Height="26px"  Text="Cancel" OnClick="_btnCancel_Click" OnClientClick="CloseOnReload()"/>
		                 	</div>   
		            	</ContentTemplate>
		        	</asp:UpdatePanel>                                       
		     	</div>  
		    
		  	</div>
     	</div>
	</div>
	</form>
</body>
</html>
