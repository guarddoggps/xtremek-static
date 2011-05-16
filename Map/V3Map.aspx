<%@ Page Language="C#" AutoEventWireup="true" CodeFile="V3Map.aspx.cs" Inherits="Map_V3Map" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
	<meta http-equiv="content-type" content="text/html; charset=UTF-8"/>

	<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <link href="../CSS/XtremeK.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/MapStyle.css" rel="stylesheet" type="text/css" />
       
	<script type="text/javascript">
		var image = null;
        var marker = new Array();

		function initialize() {
	  		var myOptions = {
		   		zoom: 10,
				center: new google.maps.LatLng(26.4307990000, -80.1724770000),
				mapTypeId: google.maps.MapTypeId.HYBRID
	  		};
	  		var map = new google.maps.Map(document.getElementById("map_canvas"),
		                            myOptions);

			point = new google.maps.LatLng(26.4307990000, -80.1724770000); 
			image = new google.maps.MarkerImage('/Icon/redcar.png', 
											   new google.maps.Size(50,50),
											   new google.maps.Point(0, 0), 
											   new google.maps.Point(15,15));
			image.size = new google.maps.Size(30, 30);
			var nMarker = new google.maps.Marker({ position: point, map: map, icon: image });
		}
	</script>
</head>

<body style="margin:0px; padding:0px;" onload="initialize()">
  <div id="map_canvas" style="width:100%; height:100%"></div>
</body>
</html>
