<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BreadCrumbs.aspx.cs" Inherits="Map_BreadCrumbs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
	<link href="../CSS/XtremeK.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/MapStyle.css" rel="stylesheet" type="text/css" />
   	<script type="text/javascript">

        var map;
        var point=null;
        var icon0 =null;
        var polyline=null;
        var myEvent=new Array();     
        var marker=new Array();
		var label;
        var hide = false;

        function evalMarker(markerScript) 
        {
            eval(markerScript);
        }

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

      function setZoom(zl, pt) 
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
                  
           map.setCenter(new GLatLng(_Lat,_Lng),zl); 
           
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
                setTimeout("setZoom("+zl+",'point')",100);
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

        function disposeMapData() 
         {
            var splitterPageWnd = window.parent;
            splitterPageWnd.disposeData(); 
        }

        function SetMapHeight() 
        {

            var H=(screen.height)*0.58;            
            document.getElementById("Map").style.height=H+"px";
        }

        function HideShowLabel()
        {
        	if(hide)
           	{
               	label.show();  
            	hide=false;                  
            }
            else if(!hide)
            {
            	label.hide();
            	hide=true;         
            }
        }
         
       function DisPoseNodes()
        {
        
        while ( marker.length > 0 ) {
                var marker1 = marker.pop();
                GEvent.clearListeners( marker1, "click" );
                marker1 = null;
            } 
 
        for(var i=0;i<marker.length;i++)
            {
               if(marker[i]!=null)
               {
                 GEvent.clearInstanceListeners(marker[i]);
                 marker[i]=null;
               }
            }
            
            
            for(var i=0;i<myEvent.length;i++)
            {
               if(myEvent[i]!=null)
               {
                 myEvent[i]=null;
               }
            }
            
            if(point!=null)
            {
                point=null;
            }
            if(icon0!=null)
            {
                icon0=null;
            }
            
            if(polyline!=null)
            {
                polyline=null;
            }
            
             
            
        }
        
        
        
   </script>
    <style type="text/css">
        
        html, body,form 
        {
            margin: 0;
            padding: 0;
            height: 100%;          
            
        }
    </style> 
</head>
<body onunload="GUnload();">
    <form id="form1" runat="server" >
    <script src="../Js/elabel.js" type="text/javascript"></script>
    <div>
        
         <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="300">
         </asp:ScriptManager>    
    
        <asp:HiddenField ID="_Zoom" runat="server" />
        <asp:HiddenField ID="_Points" runat="server" />
        
        <div id="Map" style="width:100%;height:100%">            
        </div>               
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            
                <asp:Timer ID="Timer1" runat="server" Interval="15000">
                </asp:Timer>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
            
    </form>
</body>
</html>
