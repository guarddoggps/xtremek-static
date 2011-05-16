<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HistoricalMap.aspx.cs" Inherits="Map_Historical" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Historical</title>
    
    <script type="text/javascript" src="../Js/rs.js"></script>
    <link href="../CSS/HistoricalPage.css" rel="stylesheet" type="text/css" />   

    <script type="text/javascript">
      
      var map;
       var gmarkers=new Array();
       var htmls=new Array();
       var point=null;
       var myEvent=new Array(); 
       var marker=new Array(); 
       var label=new Array(); 
       var polyline=null;
		var icon0 = null;
        var hide = false;
        
       
       function MarkerScript()
        {
           CallServer(1," Historical");
            return false;
        }
        
        function ReceiveServerData(args, context)
        {
            try
            {  
               eval(args);
            }
            catch(E)
            {
                alert(E.description);
            }

        }
          
     
        function evalMarker(markerScript)
        {
            eval(markerScript);
        }        
        
        function getMarkerFunction()
        {
            try
            {                     
                   MarkerScript();                        
            }
            catch(E)
            {
                
            }
        }
    
        function callback(result)
        {
            try
            {
                eval(result);                
            }
            catch(E)
            {
                alert(E.description);
            }
        }
        
        function MarkerInfoShow(i)
        {   
           
          if(gmarkers[i]!=null)
                gmarkers[i].openInfoWindowHtml(htmls[i]);             
        }
        
        function HideShowLabel() {
            for(var i = 0;i < label.length; i++) {
                if(hide) {
                    label[i].show();                    
                } else if(!hide) {
                    label[i].hide();
                }
            }
            
            if (hide) {
            	hide = false;
            } else {
            	hide = true;
            }
        }
        
        
        function DisPoseNodes()
        {
            
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
            
             for(var i=0;i<marker.length;i++)
            {
               if(marker[i]!=null)
               {
                 marker[i]=null;
               }
            }
            
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
         
        
      function setAfterRefresh(zoom,Pnt)
      {
         setTimeout("setZoom("+zoom+","+Pnt+")",0);
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
                    
                    setTimeout("setZoom("+zoom+","+_pnt.toString()+")",0);
                    
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
            
        
    </script>

    <style type="text/css">
        
        html, body,form 
        {
            margin: 0;
            padding: 0;
            height: 100%;          
            
        }
         #Map
        {
            position: absolute;
            margin: 0px 0px 0px 0px;    
        }
        .style1 {background-color:#ffffff;font-weight:bold; font-size:8pt; border :2px #006699 solid;}
        
    </style> 
    
</head>

<body onunload="GUnload();" onload="printPage();">
    <form id="form1" runat="server">
    <script src="../Js/elabel.js" type="text/javascript"></script> 
    <asp:ScriptManager ID="manager" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="_hmarkerScript" runat="server" />
    <asp:HiddenField ID="_Zoom" runat="server" />
    <asp:HiddenField ID="_Points" runat="server" />
    <div>
        
        <div id="Map" style="width:100%;height:100%">
        </div>
        
    </div>
    </form>
</body>
</html>
