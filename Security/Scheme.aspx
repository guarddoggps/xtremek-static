<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false"  CodeFile="Scheme.aspx.cs" Inherits="Security_Scheme" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Security Scheme</title>
     <link href="../CSS/Security.css" rel="stylesheet" type="text/css" />
     <link href="../CSS/ajaxstyle.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript">
       function moveListItem(id,leftbox,rightbox)
        {
           
            var lftBox = id.substring(0,id.lastIndexOf('__'))+'_'+leftbox;
            var rtBox = id.substring(0,id.lastIndexOf('__'))+'_'+rightbox;
            var lst=  id.substring(0,id.lastIndexOf('__'))+'__lstBT';
            
            var leftListbox = document.getElementById(lftBox);
            var rightListbox = document.getElementById(rtBox);

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
                
                 _setHiddenValue(lst,id);
                return false; 
            }
           
        }
        
    function _setHiddenValue(_rtBox,id)
    {
        var listString ="";        
        var h = id.substring(0,id.lastIndexOf('__'));
        var selectedList = document.getElementById(_rtBox);  
             
        for(var i = 0 ; i< selectedList.options.length ; i++)
        {
            //alert(selectedList.options[i].value);
            listString += selectedList.options[i].value + " ; ";
        }
        
        if( selectedList.options.length < 1)//on March 03 2009
                 {
                   listString = -1;
                 }
                
       
        document.getElementById(h+"__hListValue").value = listString ;
        
    }
    
    function checkAll()
    {      
        var chall = document.getElementById("_chkAll");
        var chk = document.getElementById("_chkPage").elements;
        if(chall.checked == true)
        {
           
            for(var i=0; i< chk.length; i++)
            {
                chk[i].checked = true;
            }
        }
        else 
        {
            for(i = 0 ; i< chk.length ; i++)
            {
                chk[i].checked = false;
            }
        }

    }
    
    function closeIframe()
    {
        /*window.parent.document.location.reload(true);
        window.parent.closeIfram(); */       
    }
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">

    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="_grdScheme">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="_grdScheme" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

		<asp:ScriptManager runat="server" AsyncPostBackTimeout="300"></asp:ScriptManager>
                        
                    
      	<telerik:RadGrid runat="server" OnItemDataBound="_grdScheme_ItemBound"  
        				AutoGenerateColumns="false" ID="_grdScheme"  Skin="Default"
                        OnItemCommand="_grdScheme_ItemCommand" 
                        onupdatecommand="_grdScheme_UpdateCommand">
       		<MasterTableView CommandItemDisplay="Top">
            	<Columns>
                	<telerik:GridEditCommandColumn>
                   	</telerik:GridEditCommandColumn>

                    <telerik:GridButtonColumn AutoPostBackOnFilter="true" Text="Delete" CommandName="Delete" UniqueName ="Delete">
                 	</telerik:GridButtonColumn>

                  	<telerik:GridTemplateColumn HeaderText="Scheme Name" HeaderStyle-Font-Names="Trebuchet MS">
                    	<ItemTemplate>
                        	<div>
                             	<asp:Label ID="lblSchemeName" runat="server" Text='<%# Eval("schemeName") %>'></asp:Label> 
                                <asp:Label ID="lblSchemeID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                          	</div>
                     	</ItemTemplate>
                 	</telerik:GridTemplateColumn>

          			<telerik:GridTemplateColumn HeaderText="Total Users" HeaderStyle-Font-Names="Trebuchet MS">
                    	<ItemTemplate>
                        	<div>
                            	<asp:Label ID="_lblUserCount" runat="server"></asp:Label>
                           	</div>
                     	</ItemTemplate>
             		</telerik:GridTemplateColumn>
                                
                	<telerik:GridTemplateColumn HeaderText="Total Units"  HeaderStyle-Font-Names="Trebuchet MS">
                    	<ItemTemplate>
                        	<div class="grdAlignmentUnits">
                             	<asp:Label ID="_lblUnitCount" runat="server"></asp:Label>
                          	</div>
                      	</ItemTemplate>
                 	</telerik:GridTemplateColumn>                                        
                                            
              	</Columns>

				<EditFormSettings EditFormType="WebUserControl"  UserControlName="SchemeEdit.ascx" >
                 	<EditColumn UniqueName="EditCommandColumn1">
                  	</EditColumn>
             	</EditFormSettings>
                            
               	<RowIndicatorColumn Visible="False">
                	<HeaderStyle Width="20px" />
                    <ItemStyle CssClass="Gridtext" />
               	</RowIndicatorColumn>
                                    
         	</MasterTableView>
    	</telerik:RadGrid>
    </div>

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

	<asp:UpdatePanel ID="_msgLbl" runat="server" >
   		<ContentTemplate>
        	<div>
            	<asp:Label ID="_lblMsg" runat="server" ></asp:Label>
        	</div>
    	</ContentTemplate>
    </asp:UpdatePanel>
            
    </form>
</body>
</html>
