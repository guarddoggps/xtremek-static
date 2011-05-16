<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SafetyZoneSetup.aspx.cs" Inherits="Tracking_SafetyZoneSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Safety Zone Setup</title>
<script src="../Js/rs.js" type="text/javascript"></script>
<link href="../CSS/SafetyZone.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" language="JavaScript">

        var myMap = null;
        var clear=false;
        var markerText=null;
        var metric = false;
        var singleClick = false;
        var hasGeofence = false;
        var cPoint = null;
    
            function evalMarker(MarkerText)
            {
                eval(MarkerText);
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
         
         function chkFocusText(cntl)
         {
            
            if(cntl.value=="Enter Zone Name")
            {
                cntl.value="";
            }
         }
         
         function chkBlurText(cntlTxt)
         {
            if(cntlTxt.value=="")
            {
                cntlTxt.value="Enter Zone Name";
            }
         }
         function loadControls()
         {
            
//            var rad = document.getElementById("_Radius");
//            var gName=document.getElementById("_txtGeofenceName");
//            var _Lat=document.getElementById("_Lat");
//            var _Long=document.getElementById("_Long");
            
//            myMap = new GMap2(document.getElementById('map')); 
         
           // if(_Lat.value=="")
            {               
//                myMap.addControl(new GSmallMapControl());
//                myMap.addControl(new GMapTypeControl());
//                myMap.enableScrollWheelZoom();
//                myMap.setCenter(new GLatLng(43.907787,-79.359741),15,G_HYBRID_MAP);
//                GEvent.addListener(myMap,'click',clickedMap);
                
            }
           // else
            {
//                myMap.addControl(new GSmallMapControl());
//                myMap.addControl(new GMapTypeControl());
//                myMap.enableScrollWheelZoom();
//                myMap.setCenter(new GLatLng(_Lat.value,_Long.value),15,G_HYBRID_MAP);
//                GEvent.addListener(myMap,'click',clickedMap);
//                
//                var _radius=((rad.value/1.60934)*5280)/3.2808399;            
//                createCircle(new GLatLng(_Lat.value,_Long.value),_radius);
//                clear=true;
            
            }
            
        }

        function createGeofence() {
            if(clear==false)
            {
                if (cPoint) {
                    createCircle(new GLatLng(cPoint.y, cPoint.x), 250);
                    hasGeofence = true;
                }
            }
        }
        function clickedMap(overlay,point)
        {
           
            if(clear==false)
            {
                if (point) {                 
                  singleClick = !singleClick;   
                  createCircle(new GLatLng(point.y, point.x), 250);
                  hasGeofence = true;
                  //setTimeout("if (singleClick) createCircle(new GLatLng("+ point.y + ", " + point.x +"), 250);", 300);                  
                }
                
                clear=true;
            }
            return false;
        }
        
        function clearCircle()
        {      
            getMarkerFunction();
            var rad = document.getElementById('<%= _lblRadius.ClientID %>');
            rad.innerHTML ="";  
                         
            if (hasGeofence == true) {
                removeCircle();
                hasGeofence = false;
            }
            
            clear=false;                         
        }
             
               function getMarkerFunction()
                {
                    
                    CallServer(1," Safety Zones");
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
                     
        
         function loadGeofence(ID,name,lat,lng,radius)
        {
             
            var _radius;
            var rad = document.getElementById('_lblRadius');
            var gName=document.getElementById("_txtGeofenceName");
            var _btnSave=document.getElementById("btnSave");
            var update=document.getElementById("_Update");
            var geofenceID=document.getElementById("_geofenceID");
            
            clearCircle();
            
            _radius=((radius/1.60934)*5280)/3.2808399;
            createCircle(new GLatLng(lat,lng),_radius);   
           
            rad.innerHTML =radius+" Km"; 
            myMap.setCenter(new GLatLng(lat,lng));   
            gName.value=name; 
            _btnSave.value="Update";
            update.value="Update";
            geofenceID.value=ID;
                
        }
        
 function EndRequestHandler(sender, args) 
         {
            alert('sd');
            if(sender._postBackSettings.panelID=="updateClear|_btnClear")    
            {
                clearCircle(); 
            }
             if(sender._postBackSettings.panelID=="saveUpdate|btnSave")   
             {
                  var update=document.getElementById("_Update");
                                    
                  if(update.value=="Update")
                    update.value="save";
             }
            
         }
         function load()
                    {
                        try
                        {
                            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                        }
                        catch(E)
                        {
                        }
                    }        
</script>


    
</head>
<body class="szbody" onload="loadControls();">
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="Manager"></asp:ScriptManager>
    <div>
        <script type="text/javascript">
        
            var queryCenterOptions = new Object();
            var queryLineOptions = new Object();
           
            
            queryCenterOptions.icon = new GIcon();
            queryCenterOptions.icon.image = "../Images/centerArrow.png";
            queryCenterOptions.icon.iconSize = new GSize(20,20);
            queryCenterOptions.icon.shadowSize = new GSize(0, 0);
            queryCenterOptions.icon.iconAnchor = new GPoint(10, 10);
            queryCenterOptions.draggable = true;
            queryCenterOptions.bouncy = false;

            queryLineOptions.icon = new GIcon();
            queryLineOptions.icon.image = "../Images/resizeArrow.png";
            queryLineOptions.icon.iconSize = new GSize(25,20);
            queryLineOptions.icon.shadowSize = new GSize(0, 0);
            queryLineOptions.icon.iconAnchor = new GPoint(12, 10);
            queryLineOptions.draggable = true;
            queryLineOptions.bouncy = false;
            

            function createCircle(point, radius) {
            
                  singleClick = false;
                  geoQuery = new GeoQuery();
                  geoQuery.initializeCircle(radius, point, myMap);
                  geoQuery.render();
                 
            }

            function removeCircle() {
                geoQuery.remove();
            }

            function destination(orig, hdng, dist) {
              var R = 6371; // earth's mean radius in km
              var oX, oY;
              var x, y;
              var d = dist/R;  // d = angular distance covered on earth's surface
              hdng = hdng * Math.PI / 180; // degrees to radians
              oX = orig.x * Math.PI / 180;
              oY = orig.y * Math.PI / 180;

              y = Math.asin( Math.sin(oY)*Math.cos(d) + Math.cos(oY)*Math.sin(d)*Math.cos(hdng) );
              x = oX + Math.atan2(Math.sin(hdng)*Math.sin(d)*Math.cos(oY), Math.cos(d)-Math.sin(oY)*Math.sin(y));

              y = y * 180 / Math.PI;
              x = x * 180 / Math.PI;
              return new GLatLng(y, x);
            }

            function distance(point1, point2) {
              var R = 6371; // earth's mean radius in km
              var lon1 = point1.lng()* Math.PI / 180;
              var lat1 = point1.lat() * Math.PI / 180;
              var lon2 = point2.lng() * Math.PI / 180;
              var lat2 = point2.lat() * Math.PI / 180;

              var deltaLat = lat1 - lat2
              var deltaLon = lon1 - lon2

              var step1 = Math.pow(Math.sin(deltaLat/2), 2) + Math.cos(lat2) * Math.cos(lat1) * Math.pow(Math.sin(deltaLon/2), 2);
              var step2 = 2 * Math.atan2(Math.sqrt(step1), Math.sqrt(1 - step1));
              return step2 * R;
            }

            function GeoQuery() {

            }

            GeoQuery.prototype.CIRCLE='circle';
            GeoQuery.prototype.COLORS=["#ff0000"];
            var COLORI=0;

            GeoQuery.prototype = new GeoQuery();
            GeoQuery.prototype._map;
            GeoQuery.prototype._type;
            GeoQuery.prototype._radius;
            GeoQuery.prototype._dragHandle;
            GeoQuery.prototype._centerHandle;
            GeoQuery.prototype._polyline;
            GeoQuery.prototype._color ;
            GeoQuery.prototype._control;
            GeoQuery.prototype._points;
            GeoQuery.prototype._dragHandlePosition;
            GeoQuery.prototype._centerHandlePosition;


            GeoQuery.prototype.initializeCircle = function(radius, point, map) {
              
                this._type = this.CIRCLE;
                this._radius = radius;
                this._map = map;
                this._dragHandlePosition = destination(point, 90, this._radius/1000);
                this._dragHandle = new GMarker(this._dragHandlePosition, queryLineOptions);
                this._centerHandlePosition = point;
                this._centerHandle = new GMarker(this._centerHandlePosition, queryCenterOptions);
                this._color = this.COLORS[0];
                map.addOverlay(this._dragHandle);
                map.addOverlay(this._centerHandle);
                var myObject = this;
                GEvent.addListener (this._dragHandle, "dragend", function() {myObject.updateCircle(1);});
                GEvent.addListener (this._dragHandle, "drag", function() {myObject.updateCircle(1);});
                GEvent.addListener(this._centerHandle, "dragend", function() {myObject.updateCircle(2);});
                GEvent.addListener(this._centerHandle, "drag", function() {myObject.updateCircle(2);});
                
               var cp = document.getElementById('<%= _centerPoint.ClientID %>');
                   cp.value = point.lat() + "," + point.lng();   
                   var rd = document.getElementById('<%= _Radius.ClientID %>');
                   rd.value =this.getDistHtml(); 
                   var km2Mile=rd.value/1.60934;
                   var rad = document.getElementById('<%= _lblRadius.ClientID %>');
                   rad.innerHTML =km2Mile.toFixed(5)+" Mile";                   
                              
            }

            GeoQuery.prototype.updateCircle = function (type) {
                this._map.removeOverlay(this._polyline);
                if (type==1) {
                  this._dragHandlePosition = this._dragHandle.getPoint();
                  this._radius = distance(this._centerHandlePosition, this._dragHandlePosition) * 1000;
                  this.render();
                  
                   var km2Mile;
                   var rd = document.getElementById('<%= _Radius.ClientID %>');
                   rd.value =this.getDistHtml(); 
                   km2Mile=rd.value/1.60934;
                   var rad = document.getElementById('<%= _lblRadius.ClientID %>');
                   rad.innerHTML =km2Mile.toFixed(5)+" Mile"; 
                   
                                   
                } else {
                  this._centerHandlePosition = this._centerHandle.getPoint();
                  this.render();
                  var cp = document.getElementById('<%= _centerPoint.ClientID %>');
                  cp.value = this._centerHandlePosition.lat() + "," + this._centerHandlePosition.lng();
                  this._dragHandle.setPoint(this.getEast());
                }
            }

            GeoQuery.prototype.render = function() {
              if (this._type == this.CIRCLE) {
                this._points = [];
                var distance = this._radius/1000;
                for (i = 0; i < 72; i++) {
                  this._points.push(destination(this._centerHandlePosition, i * 360/72, distance) );
                }
                this._points.push(destination(this._centerHandlePosition, 0, distance) );
                this._polyline = new GPolygon(this._points, this._color, 1, 1, this._color, 0.2);
                this._map.addOverlay(this._polyline);
                 
              }
            }

            GeoQuery.prototype.remove = function() {
              this._map.removeOverlay(this._polyline);
              this._map.removeOverlay(this._dragHandle);
              this._map.removeOverlay(this._centerHandle);
            }

            GeoQuery.prototype.getRadius = function() {
                return this._radius;
            }

            GeoQuery.prototype.getHTML = function() {             
              
            }

             
                
            GeoQuery.prototype.getDistHtml = function() {
             
              if (metric) {
                if (this._radius < 1000) {
                  result=this._radius.toFixed(1);                 
                } else {
                  result =(this._radius / 1000).toFixed(1);                  
                }
              } else {
                var radius = this._radius * 3.2808399;
                if (radius < 5280) {
                  result =  ((radius*0.3048)/1000).toFixed(5);                 
                } else {
                  result = ((radius / 5280)*1.60934).toFixed(5);
                
                }
              }
              return result;   
            }

            GeoQuery.prototype.getNorth = function() {
              return this._points[0];
            }

            GeoQuery.prototype.getSouth = function() {
              return this._points[(72/2)];
            }

            GeoQuery.prototype.getEast = function() {
              return this._points[(72/4)];
            }

            GeoQuery.prototype.getWest = function() {
              return this._points[(72/4*3)];
            }   
           
           
            </script>      
    </div>
    
    <div>
        <asp:HiddenField ID="_centerPoint" runat="server" />
        <asp:HiddenField ID="_Radius" runat="server" />
        <asp:HiddenField ID="_Update" runat="server"  />
        <asp:HiddenField ID="_geofenceID" runat="server" />
        <asp:HiddenField ID="_Lat" runat="server" />
        <asp:HiddenField ID="_Long" runat="server" />
    </div>
    <div class="formWrap">             
      <div class="container_01">
        <div class="container_02">
          <label>
               Radius
          </label>
          <div class="cell_label">
             <asp:Label ID="_lblRadius" runat="server"></asp:Label>   
          </div>
        </div>
      </div>
      
      <div class="container_01">      
            <div class="mapsize">
                    <div id="map" style="width:100%; height:250px"></div>
            </div>
      </div>
    <div class="container_01">
         <div class="container_02">
            <div >
            <%--<asp:UpdatePanel ID="updateClear" runat="server">
                <ContentTemplate>--%>
                <div style="padding-top: 6px">
                    <asp:Button ID="_btnCreate" CssClass="formSubmitBtn" runat="server" Text="Create Zone" 
                        OnClientClick="createGeofence(); return false;" />
                    <asp:Button ID="_btnClear" CssClass="formSubmitBtn" runat="server" Text="Clear Map" 
                        OnClientClick="clearCircle(); return false;" />
                </div>
               <%-- </ContentTemplate>
            </asp:UpdatePanel>  --%>                                    
            </div> 
         </div>
    </div>       
      <div class="container_01">
        <div class="container_02">
          <label>
               Safety Zone Name
          </label>
          <div id="divName">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div class="cell_label03">
                        <asp:TextBox CssClass="form_TextBox" ID="_txtZoneName" onFocus="chkFocusText(this)" onBlur="chkBlurText(this)" runat="server">Enter Zone Name</asp:TextBox>
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
          </div>                  
        </div>
      </div>
      <div class="container_01">
        <div class="container_02">
            <label>
                   Email
            </label>
            <div id="divEmail" >
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div class="cell_label03">
                        <asp:TextBox CssClass="form_TextBox" ID="_txtEmail" runat="server" ></asp:TextBox>
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
          <div class="cell_label01" >
               <div>
                    <asp:UpdatePanel ID="updateON" runat="server">
                        <ContentTemplate>
                        <div class="radiobutton">
                            <asp:RadioButton ID="_rdoON" runat="server" GroupName="OnOff" Checked="true" />
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>                        
               </div>
               <div class="cell_label02">
                    Safety Zone On
               </div>
               <div>
                    <asp:UpdatePanel ID="updateOFF" runat="server">
                        <ContentTemplate>
                        <div class="radiobutton">
                            <asp:RadioButton ID="_rdoOFF" runat="server" GroupName="OnOff" />
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
               </div>
               <div class="cell_label02">
                    Safety Zone Off   
               </div>                  
            </div>
       </div>
    </div>
        
        <div class="container_01">
          <div class="container_02">
            <label>
                Notification Phone
            </label>
            <div>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <div class="cell_label">
                            <asp:TextBox CssClass="form_TextBox" ID="_txtPhoneNumber" runat="server"></asp:TextBox>                            
                        </div>                        
                    </ContentTemplate>
                </asp:UpdatePanel>                                    
            </div>
         </div>
        </div>
        <div class="container_01">
          <div class="container_02">
            <label>
                Enable SMS
            </label>
            <div class="cell_label">                
               <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                 <ContentTemplate>
                   <div>
                      <asp:CheckBox CssClass="alertcheckbox"  Checked="true" ID="_ckOnSMS" runat="server" 
                           AutoPostBack="True" oncheckedchanged="_ckOnSMS_CheckedChanged"/>
                   </div>
                   <div class="alertcell_label">
                        On
                   </div>
                   <div>
                      <asp:CheckBox CssClass="alertcheckbox" Checked="false" ID="_chkOffSMS" runat="server" 
                           AutoPostBack="True" oncheckedchanged="_chkOffSMS_CheckedChanged" />
                   </div>
                   <div class="alertcell_label">
                        Off
                   </div>                                        
                 </ContentTemplate>
               </asp:UpdatePanel>                                                  
            </div>
          </div>
        </div>
        
   <div class="container_01">
     <div>
        <div class="szalign">
            System will send one notification when unit enters safety zone <br />
            and one notification when unit leaves safety zone.                        
        </div> 
     </div>
   </div>
   <div class="container_01">
     <div>
        <div class="szcol01">
                Directions:<br />
                &nbsp;1. Move cursor to desired point in map.<br />
                &nbsp;2. Single click on map safety zone will appear.<br />
                &nbsp;3. Drag "size ring" to desired size of safety zone.<br />
                &nbsp;4. Click "OK" to submit. <br />   
                
                <br />
                OR
                <br />
                <br />
                
                &nbsp;1. Click "Create Zone" to create a safety zone.<br />
                &nbsp;2. Drag the safety zone to the desired position.<br />   
                &nbsp;3. Drag "size ring" to desired size of safety zone.<br /> 
                &nbsp;4. Click "OK" to submit.<br />    
                
                <br />
        </div>  
        <div class="szcol02">
            <img alt="Draw" src="../Images/geosample.gif" />   
        </div>       
     </div>
   </div>
   <div>
     <div class="alertcontainer">
        <div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                <div class="alertinstruction">
                    <asp:Label id="_lblMessage" runat="server" ></asp:Label>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>                                   
        </div> 
     </div>
    </div>
   <div class="container_01">
     <div class="container_02">
      <div id="divOK">  
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <div class="alertbutton01">
               <asp:Button ID="_btnOK" CssClass="formSubmitBtn" runat="server" Text="OK" 
                    onclick="_btnOK_Click"/>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
      </div>
      <div id="divCancel" class="alertbutton01">
            <asp:Button ID="_btnCancel" CssClass="formSubmitBtn" runat="server" Text="Cancel" OnClick="_btnCancel_Click"/>
      </div>       
     </div>
   </div>
                  
  </div> 
 </form>
</body>
</html>
