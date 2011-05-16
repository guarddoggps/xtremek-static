<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainMap.aspx.cs" Inherits="Map_MainMap" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../CSS/XtremeK.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/MapStyle.css" rel="stylesheet" type="text/css" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <script type="text/javascript">
           window.onload = function()
           {
               
           }
       </script>
   </telerik:RadCodeBlock>  
   
   <script type="text/javascript">
    
        var map;    
        var point=null;
        var icon0 =null;
        var myEvent=new Array(); 
        var marker=new Array();
        var info=new Array(); 
        var label=new Array(); 
        var zoomEvent; 
        var reDraw = 'false';
        var hide = false;

	    function MarkerScript() 
	    {
            CallServer(1," Historical");
            return false;
        }
        
        function ReceiveServerData(args, context)
        {
            eval(args);
        }
        
        function evalMarker(markerScript)
        {
            eval(markerScript);
        }
        
        setInterval("MarkerScript()",15000);
     
     function afterZoomEnd() 
     {
            if(window.opener ==null)
            {
                var splitterPageWnd = window.parent; 
                var _zoomLevel=getZoom();
                var _points=getPoints();            
                splitterPageWnd.setMapZoom(_zoomLevel);
                splitterPageWnd.setMapPoint(_points);
                document.getElementById("_Points").value=_points;
             }
      }

      function getZoom() 
      {
            try
            {               
            
             var zLevel=map.getZoom();
             document.getElementById("_Zoom").value=zLevel.toString();
             return zLevel;
            }
            catch(E)
            {
                alert(E.description);
            }
      }

      function getPoints() 
      {
            var point=map.getCenter();
            return point;            
      }


      function setAfterRefresh(zoom, Pnt) 
      {
            setTimeout("setZoom("+zoom+","+Pnt+")",1000);
      }
        
      function setZoom(zl,pt)
      {

           var point;
           
           var splitterPageWnd = window.parent;
           
           if(window.opener!=null)
           {
            point=window.opener.document.getElementById("_point").value;
           }
           
           else 
           {
              var _pnt_=splitterPageWnd.passPoint(); 
              point=_pnt_;
           }
           
           
           var _LatLngPoint=new Array();
           
           _LatLngPoint=point.split(",");
           
           var _Lat=_LatLngPoint[0].replace("(","");
           var _Lng=_LatLngPoint[1].replace(")","");

           map.setCenter(new GLatLng(_Lat, _Lng), zl);
           
           if(window.opener!=null)
           {
            window.opener.disposeData(); 
           }
           
           else
           {
            splitterPageWnd.disposeData();         
           }
                  
        }
      
       function printPage()
        {
                     
            if(window.opener!=null)
            {
               
                var zl=window.opener.document.getElementById("_zoomLevel").value;            
                setTimeout("setZoom("+zl+",'point')",0);
            }
            else
            {   
                 disposeMapData();
                 var splitterPageWnd = window.parent;
                 var zoom=splitterPageWnd.passZoom();
                 var _pnt=splitterPageWnd.passPoint();
                 
                 if(zoom!="")
                 {                    
                    
                    setTimeout("setZoom("+zoom+","+_pnt.toString()+")",100);
                    
                 }
            }
        }

        function HideShowLabel() {
            for(var i = 0;i < label.length; i++) {
                if(hide)
                {
                    label[i].show();                    
                }
                else if(!hide)
                {
                    label[i].hide();
                }
                
                if(i ==label.length-1)
                {
                    if(hide)
                        hide=false;
                     else
                        hide=true;
                        
                }
            }
        }

        
         function disposeMapData()
        {
            var splitterPageWnd = window.parent;
            splitterPageWnd.disposeData(); 
        }
        
         function disPoseEvent()
        {
            
//            if(zoomEvent!=null)
//            {
//                GEvent.removeListener(zoomEvent);
//            }
//            
//            for(var i=0;i<myEvent.length;i++)
//            {
//               if(myEvent[i]!=null)
//               {
//                 GEvent.removeListener(myEvent[i]);
//                 myEvent[i]=null;
//               }
//            }
            
            
            
//            if(point!=null)
//            {
//                point=null;
//            }
//            if(icon0!=null)
//            {
//                icon0=null;
//            }
//            
//             for(var i=0;i<marker.length;i++)
//            {
//               if(marker[i]!=null)
//               {
//                
//                 map.removeOverlay(marker[i]);
//                 marker[i]=null;
//               }
//            }            
        }
        
        function SetMapHeight()
        {
            var H=(screen.height)*0.58;            
            document.getElementById("Map").style.height=H+"px";
        }
        
        function hideLoadImage()
        {   
                 
            var splitterPageWnd = window.parent;
            splitterPageWnd.hideLoadingImage(); 
        }
        
   </script>
   <style type="text/css">
      
        html { height: 99%; }
        body { min-height: 100%; height: 100%;  margin: 0;
            padding: 0; }

    </style>
    
    
</head>
<body onload=" printPage();" onunload="GUnload();"z>
    <form id="form1" runat="server">
    <script src="../Js/elabel.js" type="text/javascript"></script>

    <div>
    
            <asp:HiddenField ID="_Zoom" runat="server" />
            <asp:HiddenField ID="_Points" runat="server" />
            <asp:HiddenField ID="_markersArray" runat="server" />
        
            <asp:ScriptManager ID="_scriptManager" runat="server" AsyncPostBackTimeout="300"></asp:ScriptManager>
    
               <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"  OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                   <AjaxSettings>
                   
                       <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                           <UpdatedControls>
                               <telerik:AjaxUpdatedControl ControlID="Panel1" LoadingPanelID="RadAjaxLoadingPanel2" />
                           </UpdatedControls>
                       </telerik:AjaxSetting>
                       
                   </AjaxSettings>
            </telerik:RadAjaxManager>
            
            <asp:Panel ID="Panel1" runat="server">
            
               <asp:Panel ID="Panel2" Visible="false" runat="server">
                My content:
              </asp:Panel>
              
            </asp:Panel>
            
             <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2"  runat="server" Height="75px" Width="75px" > 
                <div class="TransparentGrayBackground"></div>
                
                    <div class="Sample5PageUpdateProgress">
                        <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>' style="border:0;" /> 
                        &nbsp;Loading...
                    </div>
                    
                
                
            </telerik:RadAjaxLoadingPanel>--%> 
        
        
        
           <%--<div id="dvLoad">
           
            <div class="TransparentGrayBackground"></div>
                
                    <div class="Sample5PageUpdateProgress">
                        <asp:Image  ID="ajaxLoadNotificationImage1" 
                                    runat="server" 
                                    ImageUrl="../Images/loading.gif" 
                                    AlternateText="[image]" />
                        &nbsp;Loading...
                    </div>
                               
           </div>--%>
      
        
        
              <div id="Map" style="width:100%;height:100%">            
              </div>
              
              <%--<asp:UpdatePanel ID="_updateMap"         runat="server">
                <ContentTemplate>
                    <asp:Timer ID="_timer" runat="server" Interval="15000"></asp:Timer>
                    <asp:Label ID="messageLabel" runat="server"></asp:Label>
                </ContentTemplate>
              </asp:UpdatePanel>--%>
              
              
              <%--<asp:UpdateProgress ID="siteUpdateProgress" DisplayAfter="50" AssociatedUpdatePanelID="_updateMap" runat="server">
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
              </asp:UpdateProgress>--%>
              
              <%--<asp:UpdatePanel ID="UpdateShowbutton" UpdateMode="Always" runat="server">
			    <ContentTemplate>
					    <div >
                            <asp:Label ID="btnShowRpt" runat="server" Visible="true" Text="loading.."/>   
                            <img src="../Images/loading.gif" alt="..." width="23" height="23"/>          
					    </div>
				</ContentTemplate>
		    </asp:UpdatePanel>--%>
		    <%--<div >
				<asp:UpdateProgress ID="UpdateProgress1" DynamicLayout="true" runat="server"  AssociatedUpdatePanelID="UpdateShowbutton">
				  <ProgressTemplate>
				    <img src="../Images/loading.gif" alt="..." width="23" height="23"/>
				  </ProgressTemplate>
				</asp:UpdateProgress>
			</div>--%>
              

              
              
    </div>   
       
    </form>
</body>
</html>
