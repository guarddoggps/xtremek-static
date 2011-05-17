<%@ Page Language="C#"  AutoEventWireup="true" ValidateRequest="false" CodeFile="Home.aspx.cs" Inherits="Home" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GuardDog GPS Tracking - Map</title>
    <link href="CSS/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="CSS/TreeCSS.css" rel="stylesheet" type="text/css" />
    <link href="MySkin/Splitter.MySkin.css" rel="stylesheet" type="text/css" />
    <link href="MySkin/TreeView.MySkin.css" rel="stylesheet" type="text/css" />
    <link href="MySkin/Menu.MySkin.css" rel="stylesheet" type="text/css" />
    <link href="Aeromazing/Window.Aeromazing.css" rel="stylesheet" type="text/css" />

    <script src="Js/rs.js" type="text/javascript"></script>
    <script src="Js/DateFormat.js" type="text/javascript"></script>
	  <script type="text/javascript" src="Js/jquery-1.js"></script>
	  <script type="text/javascript" src="Js/hovertip.js"></script>
	  <script type="text/javascript" src="Js/excanvas.js"></script>
	  <script type="text/javascript" src="Js/coolclock.js"></script> 
	  <script type="text/javascript" src="Js/moreskins.js"></script> 
    <script type="text/javascript">
      
   	function onClientNodeClickedHandler(sender, eventArgs) 
    {
    	var node = eventArgs.get_node();
        node.toggle();
        var nodeStr=node.get_text();        
                
        if(nodeStr.search("Refresh")!=-1)
        {
         	return true;
        }

        eventArgs.set_cancel(!node.get_expanded());                  
  	}  
            
    function GetRadWindow()
    {
    	var oWindow = null;
        if (window.radWindow) {
			oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
       	} else if (window.frameElement.radWindow) {
			oWindow = window.frameElement.radWindow;//IE (and Moz az well)
            return oWindow;
        }
   	}

    function show()
    {
       	try
       	{
           	document.getElementById("lblLocalTime").innerHTML ="Local Time: "+getDateFormat(true);
          	document.getElementById("lblUTCTime").innerHTML = "UTC Time: "+getDateFormat(false);
		}
      	catch(E)
      	{
                
  		}                
 	}
        
 	function ReLoadWindow(url,title,width,height) 
   	{ 
    	var manager = GetRadWindowManager();               
      	var cWindow = manager.getActiveWindow();
               
       	if(cWindow != null)
         	cWindow.close();
               
       	var oWindow = radopen (url, null);
       	oWindow.SetSize (width,height);
     	oWindow.MoveTo(195,130);
       	oWindow.set_title(title);
       	oWindow.add_pageLoad(function(){oWindow.set_status("  "); });                                         
   	} 
            
	function GetHelpText(text)
  	{
      	CallServer(text,"Help Text");
      	return false;
  	}
        
  	function ReceiveServerData(args, context)
  	{
       	// document.getElementById("_lblFooter").innerHTML =args; 
   	}     

    </script>
  	<style type="text/css"> 
     	html,body,form 
		{ 
        	background:#FDB94E; 
        	height:100%; 
        	margin:0; 
        	padding:0; 
        	width:100%;
        	position:absolute;
   		}

		.splitbar
		{
			background: gray;
		}
	</style> 
</head>

<body scroll="no">
	<form id="form1" runat="server">
    <asp:ScriptManager ID="_scriptManager" runat="server"  AsyncPostBackTimeout="300"></asp:ScriptManager>
    
     <asp:HiddenField ID="_zoomLevel" runat="server" />
     <asp:HiddenField ID="_point" runat="server" />
     <asp:HiddenField ID="_Url" runat="server" />
     <asp:HiddenField ID="_hdsk" runat="server" />
     <asp:HiddenField ID="_htmls" runat="server" />
     <asp:HiddenField ID="_gmarkers" runat="server" />
        
    <input id="deviceID" runat="server" type="hidden"></input>
    <input id="unitID" runat="server" type="hidden"></input>

    <script type="text/javascript">
    
    setInterval("show()",1000);
    
	Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

	function EndRequestHandler(sender, args)
	{
    	if (args.get_error() != undefined)
      	{
        	var errorMessage;
           	if (args.get_response().get_statusCode() == '0')
         	{
         		args.set_errorHandled(true);
        	}
           	else
         	{
            	// not my error so let the default behavior happen      
         	}
  		}
	}
            
    function setMapZoom(_zoomLevel)
 	{
        var splitter =$find("<%= SplitterMap.ClientID %>");
        var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
        var iframe = pane.getExtContentElement(); 
     	var href=iframe.contentWindow.document.location;
       	var url=href.toString();
     	document.getElementById("_Url").value=url;
     	document.getElementById("_zoomLevel").value=_zoomLevel;
 	}

  	function setMapPoint(_point)
 	{
     	document.getElementById("_point").value=_point;            
  	}
        
  	function disposeData()
   	{           
        var splitter =$find("<%= SplitterMap.ClientID %>");
        var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
        var iframe = pane.getExtContentElement(); 
       	var href=iframe.contentWindow.document.location;
      	var url=href.toString();
    	var prevUrl=document.getElementById("_Url").value;
                             
    	if(prevUrl!=url)
      	{
        	document.getElementById("_zoomLevel").value="";
         	document.getElementById("_point").value="";                    
        	document.getElementById("_Url").value="";
       	}
 	}
        
   	function passZoom()
   	{
		return document.getElementById("_zoomLevel").value;
  	}
        
  	function passPoint()
   	{
    	return document.getElementById("_point").value;
   	}
 
	function SetHistoricalMap()
  	{
    	try
      	{
            var splitter =$find("<%= SplitterMap.ClientID %>");
            var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
            var iframe = pane.getExtContentElement(); 
          	var href=iframe.contentWindow.document.location;
           	var url=href.toString(); 
                                      
          	if(url.search("MainMap")!=-1) {
            	url=url.replace('MainMap','HistoricalMap');                            
              	pane.set_contentUrl(url); 
                           
         	}
         	else if(url.search("BreadCrumbs")!=-1) {
            	url=url.replace("BreadCrumbs","HistoricalMap");                            
              	pane.set_contentUrl(url); 
          	} else if(url.search("HistoricalMap")!=-1) {
           		HGMapMarkerShow();
		   	}   
  		}
       	catch(E)
      	{
       		alert(E.dexcription);
        }             
	}
    
	function HGMapMarkerShow()
    {
    	var splitter =$find("<%= SplitterMap.ClientID %>");
       	var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
      	var iframe = pane.getExtContentElement(); 
        var contentWindow = iframe.contentWindow;                                          
       	contentWindow.getMarkerFunction();                        
 	}
                
	function HideShowLabel(imgObj)
   	{
    	try
       	{
            var splitter =$find("<%= SplitterMap.ClientID %>");
            var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
            var iframe = pane.getExtContentElement(); 
           	var contentWindow = iframe.contentWindow;                        
        	contentWindow.HideShowLabel();     
          	if(imgObj.src.search("On") !=-1)
          	{
            	imgObj.src = imgObj.src.replace("On","Off");
          	} else {
            	imgObj.src = imgObj.src.replace("Off","On");
      		}
		}
        catch(E)
        {
                    
        }
	}

	function MarkerShow(i)
  	{
       	var splitter =$find("<%= SplitterMap.ClientID %>");
        var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
        var iframe = pane.getExtContentElement(); 
       	var contentWindow = iframe.contentWindow;                        
       	contentWindow.MarkerInfoShow(i);     
   	}   

	function PrintPane ()
    {
		try
        {       
            var splitter =$find("<%= SplitterMap.ClientID %>");
            var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
            var iframe = pane.getExtContentElement(); 
          	var contentWindow = iframe.contentWindow;                        
          	zlevel=contentWindow.getZoom(); 
                        
          	document.getElementById("_zoomLevel").value=zlevel.toString();
           	document.getElementById("_point").value=contentWindow.getPoints();

         	var cssFileAbsPath = location.href.substr(0, location.href.toString().lastIndexOf('/') + 1) +'CSS/printStyle.css';
           	var arrExtStylsheetFiles = [cssFileAbsPath];  
                        
          	var url=contentWindow.location.toString(); 
           	contentWindow.print();
                    
     	}
       	catch(E)
        {
        	alert(E.description);
     	}
                    
	}
    
	function refreshPane(paneID)
   	{
    	try
       	{
            var splitter =$find("<%= SplitterMap.ClientID %>");
            var pane = splitter.getPaneById("<%= MapPane.ClientID %>");
            var iframe = pane.getExtContentElement(); 
           	var url = iframe.contentWindow.document.location;    
           	var contentWindow=iframe.contentWindow;
                        
        	document.getElementById("_zoomLevel").value=contentWindow.getZoom(); 
    		document.getElementById("_point").value=contentWindow.getPoints();

         	iframe.src = url;
  		}
     	catch(E)
     	{
         	alert(E.description);
      	}
                   
	}
	
    function checkSelectedUnit()
    {
        if (!document.deviceID || !document.unitID) {
            alert('Please select a unit from the units menu in order to use this feature.');
            return false;
        }

        return true;
    }

    function redAlertsItemClicked()
    {
        if (checkSelectedUnit()) {
            ReLoadWindow("Tracking/RedAlerts.aspx?unitID=" + document.unitID,'Red Alerts Setup',440, 330);
        }
    }

    function speedingItemClicked()
    {
        if (checkSelectedUnit()) {
            ReLoadWindow("Tracking/Speeding.aspx?unitID=" + document.unitID,'Speeding Alerts Setup',450, 410);
        }
    }

    function safetyZonesItemClicked()
    {
        if (checkSelectedUnit()) {
            ReLoadWindow("Tracking/SafetyZones.aspx?unitID=" + document.unitID,'Safety Zones Setup',550, 500);
        }
    }

    function historyItemClicked()
    {
        if (checkSelectedUnit()) {
            ReLoadWindow("Tracking/Historical.aspx?deviceID=" + document.deviceID,'Unit History',530,455);
        }
    }

    function miniMapItemClicked()
    {
        if (checkSelectedUnit()) {
            ReLoadWindow("Map/MiniMap.aspx?deviceID=" + document.deviceID,'Minimap',380, 580);
        }
    }
    
    function remoteControlItemClicked()
    {
        if (checkSelectedUnit()) {
            ReLoadWindow("RemoteControl.aspx?unitID=" + document.unitID,'Control Unit Remotely',390,472);
        }
    }

	var last_val = "";
	
	function isRootMenu(val)
	{
	   if (val == "UnitsMenu" || val == "AdminMenu" || 
	       val == "ReportsMenu" || val== "FleetMenu") {
	        return true;
	   }
	   
	   return false;
	}
	
	function RadMenu1ItemClicking(sender, args)
	{
		var menu = $find("<%= RadMenu1.ClientID %>");
  		var item = args.get_item();
  		
	    if (isRootMenu(item.get_value())) {
	        var item = args.get_item();
	        if (last_val != "" && last_val != item.get_value()) {				
  				 menu.close();
	        }
	       
	        last_val = item.get_value();
	    }
	}
	
  	function RadMenu1ItemClicked(sender, args)
	{
		var menu = $find("<%= RadMenu1.ClientID %>");
  		var item = args.get_item();

	    
        if (item.get_text() == "Remote Control") {
            remoteControlItemClicked();
            return;
        }

  		var deviceID = item.get_attributes().getAttribute("deviceID");
		
		if (deviceID != null || item.get_value() == "Admin" || 
								item.get_value() == "Reports") {
			menu.close();
		}
		
		if (deviceID != null) {   
            document.deviceID = deviceID;
  			var unitID = item.get_attributes().getAttribute("unitID");
            document.unitID = unitID;
		} 
	}
	
	
	function RadMenu1MouseOver(sender, args)
	{  		
	    if (isRootMenu(args.get_item().get_value())) {
	        sender.set_clicked(false);
	    }
	}

	</script>
 
<div class="page"> 
  <div class="header">	
	  <div id="logo">
	    <a href="http://guarddoggps.com/"><img src="Images/guard-dog-logo-trans.png" alt="Guard Dog Logo" border="0"/></a>
	  </div>

      	<%--<div class="timeDiv">
        	<div>
				<asp:Label ID="lblLocalTime" runat="server"></asp:Label>				    
			</div>
			<div>
				<asp:Label ID="lblUTCTime" runat="server"></asp:Label>
			</div>                        
       	</div>--%>

		<div class="header_buttons">
			<!--<div class="welcome">Welcome: <asp:Label ID="_lblUserName" runat="server"></asp:Label></div>-->
			
			<div class="header_button">				
				<asp:ImageButton ID="_btnEmail" runat="server" OnClientClick="ReLoadWindow('CompanyAdmin/ContactUs.aspx','Contact Us',460,450); return false;" ImageUrl="Images/mail_up.jpg" onmouseover="this.src='Images/mail_over.jpg';" onmouseout="this.src='Images/mail_up.jpg';"/>			             	    
			</div>	
			<div class="header_button">				
				<asp:ImageButton ID="_btnPrint" runat="server" OnClientClick="PrintPane();return false;" ImageUrl="Images/print_up.jpg" onmouseover="this.src='Images/print_over.jpg';" onmouseout="this.src='Images/print_up.jpg';"/>			             	    
			</div>	
				
			<div class="header_button">				
				<asp:ImageButton ID="_btnRefresh" runat="server" OnClientClick="refreshPane('MapPane'); return false;" ImageUrl="Images/refresh_up.jpg" onmouseover="this.src='Images/refresh_over.jpg';" onmouseout="this.src='Images/refresh_up.jpg';"/>			             	    
			</div>	
				
			<div class="header_button">				
				<asp:ImageButton ID="_btnTagOnOff" runat="server" OnClientClick="HideShowLabel(this);return false;" ImageUrl="Images/tag_up.jpg" onmouseover="this.src='Images/tag_over.jpg';" onmouseout="this.src='Images/tag_up.jpg';"/>			             	    
			</div>	
		</div>
			
		<div class="logout">
			<asp:LinkButton ID="_lnkLogOut" CssClass="logout" runat="server" OnClick="Logout_Click" Text="Logout"></asp:LinkButton>
		</div>	
	</div> 

	<div class="masterd">

		<telerik:RadWindowManager Visible="true" ID="Singleton"  CssClass="body_bg" EnableAjaxSkinRendering="true" Behaviors="Default" VisibleOnPageLoad="false" runat="server" >
		</telerik:RadWindowManager>

		<telerik:RadSplitter ID="RadSplitter1" Width="100%" Height="100%" SplitBarsSize="0" EnableEmbeddedSkins="false" Skin="MySkin" runat="server" BorderWidth="0px" BorderSize="0" PanesBorderSize="0" ResizeMode="EndPane" Orientation="Horizontal">     

      <telerik:RadPane CssClass="OverFlow" ID="TestPane" Scrolling="None" BorderWidth="10" Width="100%" runat="server">
				<telerik:RadSplitter ID="SplitterMap" Width="100%"  EnableEmbeddedSkins="false" Skin="MySkin" SplitBarsSize="2" runat="server" Orientation="Vertical">                        
					<%--<telerik:RadPane ID="TreeViewPane" Collapsed="true" Width="200px" runat="server" >
						<asp:UpdatePanel ID="UpdatePanel1" runat="server">
							<ContentTemplate>
								<div id="unitTree">
								 	<telerik:RadTreeView CssClass="treeView" EnableEmbeddedSkins="false" Skin="MySkin" ShowLineImages="false" RetainScrollPosition="true" ID="RadTreeView1" runat="server" SingleExpandPath="true" OnClientNodeClicking="onClientNodeClickedHandler">                                                            
								  	</telerik:RadTreeView>
								</div>                                                    
							</ContentTemplate>
					   	</asp:UpdatePanel>
					</telerik:RadPane>--%>
				
					<%--<telerik:RadSplitBar ID="splitLeft" CssClass="splitbar" runat="server" CollapseMode="Forward" />--%>

					<telerik:RadPane ID="MapPane" CssClass="map" Width="100%" runat="server" ContentUrl="Map/MainMap.aspx"></telerik:RadPane>
		    	</telerik:RadSplitter> 
		 	</telerik:RadPane>  

      <telerik:RadPane ID="bar" Height="72px" CssClass="footer" BorderWidth="0" Scrolling="None" runat="server">


				<div class="footer_buttons">	

                    <asp:Timer ID="_Timer" runat="server" Interval="30000" OnTick="_Timer_Tick"></asp:Timer>

					<asp:UpdatePanel ID="MenuUpdatePanel" UpdateMode="Conditional" runat="server">               
						<Triggers>
                			<asp:AsyncPostBackTrigger ControlID="_Timer" EventName="Tick" />
            			</Triggers>
                             
						<ContentTemplate>    
							<telerik:RadMenu runat="server"  
											 ID="RadMenu1" 
											 EnableEmbeddedSkins="false" 
											 Skin="MySkin" 
											 ClickToOpen="true" 
											 OnClientItemClicked="RadMenu1ItemClicked" 
											 OnClientItemClicking="RadMenu1ItemClicking"
											 OnClientMouseOver="RadMenu1MouseOver">
											 
                                <ExpandAnimation Type="OutQuad" Duration="50" />
                                <CollapseAnimation Type="OutQuad" Duration="50" />
								<Items>					        
									<telerik:RadMenuItem CssClass="footer_button" ID="menuUnits" Value="UnitsMenu" ImageUrl="Images/unit_up.png" >																
									</telerik:RadMenuItem>        

									<telerik:RadMenuItem CssClass="footer_button" ID="menuAdministration" Value="AdminMenu" ImageUrl="Images/admin_up.png">
								     	<Items>
											<telerik:RadMenuItem Text="Company Admin" CssClass="menuHeader" OnClick="return false;"/>
											<telerik:RadMenuItem Text="User Management" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/UserManagement.aspx','User Management',468,500)"/>
											<telerik:RadMenuItem Text="Unit Management" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/UnitManagement.aspx','Unit Management',700,450)"/>
											<telerik:RadMenuItem Text="Unit Category" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/UnitType.aspx','Unit Category',450,450)"/>
											<telerik:RadMenuItem Text="User Group" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/UserGroup.aspx','User Group',450,450)"/>
											<telerik:RadMenuItem Text="Geofence Setup" Value="Admin" OnClick="ReLoadWindow('Map/Geofence.aspx','Geofence Setup',540,450)"/>
											<telerik:RadMenuItem Text="Rules Setup" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/Rules.aspx','Rules Setup',450,450)"/>
											<telerik:RadMenuItem Text="Security Scheme" Value="Admin" OnClick="ReLoadWindow('Security/Scheme.aspx','Security Scheme',700,450)"/>
											<telerik:RadMenuItem Text="Ad Image" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/AdImage.aspx','Advertising Image',680,400)"/>
											<telerik:RadMenuItem Text="User & Unit List" Value="Admin" OnClick="ReLoadWindow('CompanyAdmin/UserUnitList.aspx','User & Unit List',455,450)"/>

											<telerik:RadMenuItem Text="Unit Alerts Setup" CssClass="menuHeader" OnClick="return false;"/>
											<telerik:RadMenuItem Text="Red Alerts" Value="" OnClick="redAlertsItemClicked(); return false;"/>
											<telerik:RadMenuItem Text="Speeding" Value="" OnClick="speedingItemClicked(); return false;"/>
											<telerik:RadMenuItem Text="Safety Zones" Value="" OnClick="safetyZonesItemClicked(); return false;"/>
										</Items>
									</telerik:RadMenuItem>
								
									<telerik:RadMenuItem CssClass="footer_button" ID="_btnReports" Value="ReportsMenu" ImageUrl="Images/report_up.png">                
										<Items>											
											<telerik:RadMenuItem Text="Unit Reports" CssClass="menuHeader" OnClick="return false;"/>
											<telerik:RadMenuItem Text="History" Value="Reports" OnClick="historyItemClicked(); return false;"/>
										</Items>
									</telerik:RadMenuItem>

									<telerik:RadMenuItem CssClass="footer_button" ID="menuFleet" Value="FleetMenu" ImageUrl="Images/fleet_up.png">
										<Items>											
											<telerik:RadMenuItem Text="Fleet Management" Value="Admin" CssClass="menuHeader" OnClick="return false;"/>
											<telerik:RadMenuItem Text="Supplies" Value="Admin" OnClick="ReLoadWindow('FleetManagement/Supplies.aspx','Supplies Management',650,450)"/>
											<telerik:RadMenuItem Text="Pattern" Value="Admin" OnClick="ReLoadWindow('FleetManagement/Pattern.aspx','Pattern Management',670,450)"/>
											<telerik:RadMenuItem Text="Initiate Maintenance" Value="Admin" OnClick="ReLoadWindow('FleetManagement/InitiateMaintenance.aspx','Initiate Maintenance',450,450)"/>
											<telerik:RadMenuItem Text="Maintenance Status" Value="Admin" OnClick="ReLoadWindow('FleetManagement/MaintenanceStatus.aspx','Maintenance Status',650,450)"/>
										</Items>	
									</telerik:RadMenuItem>
								</Items>
							</telerik:RadMenu>                                  
						</ContentTemplate>
					</asp:UpdatePanel>	
				</div>	
 
				<canvas style="float: right;" id="clock" class="CoolClock:swissRail:34"></canvas>

		    </telerik:RadPane>        
        </telerik:RadSplitter>    

		<%--<embed style="position: absolute; bottom: 2px; right: 6px;" id=svgembed src="clock.svg" pluginspage="http://www.adobe.com/svg/viewer/install/"  type="image/svg+xml" runat="server" />--%>


	</div>
	</div> 

    </form>
</body>
</html>
