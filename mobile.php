<?php

$GLOBALS['RESTmap'] = array();
$GLOBALS['RESTmap']['GET'] = array('getUnitStatus' => 'getUnitStatus',
                                   'getUserID' => 'getUserID',
                                   'getUnits' => 'getUnits',
                                   'getUnitInfo' => 'getUnitInfo',
                                   'getHasAlert' => 'getHasAlert'
                                   );
$GLOBALS['RESTmap']['POST'] = array('lock' => 'lock',
                                    'unlock' => 'unlock',
                                    'silentAlarm' => 'silentAlarm',
                                    'panicAlarm' => 'panicAlarm',
                                    'stopEngine' => 'stopEngine',
                                    'startEngine' => 'startEngine',
                                    'auxilary' => 'auxilary');
$GLOBALS['RESTmap']['PUT'] = array('my-put' => 'my_put');
$GLOBALS['RESTmap']['DELETE'] = array('my-delete' => 'my_delete');

RunServer();

function getResult($query) {
    return getDbResult("xtremek", $query);
}

function getDbResult($dbname, $query) {
    $dbconn = pg_connect("host=172.27.12.132 dbname=$dbname user=xtremek password=xtremek0753") 
                or die('Could not connect: ' . pg_last_error());
    
    $result = pg_query($dbconn, $query) or die('Query failed: ' . pg_last_error());
    
    $rows = pg_num_rows($result);
    $lines = array();
    for ($i = 0; $i < $rows; $i++) {
        $line = pg_fetch_array($result, $i, PGSQL_ASSOC);
        $lines[] = $line;
    }
    
    pg_free_result($result);
    pg_close($dbconn);
    
    return $lines;
}

function encrypt_des($password) {
    $dbconn = pg_connect("host=192.168.12.132 dbname=crypt user=xtremek password=xtremek0753");
    
    
    $query = "select encrypt_des('" . $password . "') as rc;";
    $result = pg_query($dbconn, $query)
                                or die('Query failed: ' . pg_last_error());
                                
                                
    $rows = pg_num_rows($result);
    
    if ($rows < 1) {
        return null;
    }
    
    $line = pg_fetch_array($result, null, PGSQL_ASSOC);
    
    return $line['rc'];
}

function getReverseGeocoding($lat, $lon) {
    if ($lat == null || $lon == null) {
        return;
    }
    
    $query = "select * from findNearestAddress(" . $lat . "," . $lon . ");";
   
    $result = getDbResult("reversegeocoding", $query);
    
    if (count($result) > 0) {
        echo "Zip Code: " . $result[0]['postalcode'] . "\n";
        echo "City: " . $result[0]['placename'] . "\n";
        echo "County: " . $result[0]['adminname2'] . "\n";
        echo "State: " . $result[0]['adminname1'] . "\n";
        echo "Country: " . $result[0]['countrycode'] . "\n";
    }
}

function getUnitStatus($data) {
    $unitID = $data['unitID'];
    if ($unitID == null) {
        return -1;
    }
    
    header('Content-type: text/plain');
        
    $strSQL = "select * from tblAlert where unitID = " . $unitID . " order by alerttime desc limit 1;";
    $result = getResult($strSQL);
    
    if (count($result) > 0) {
        echo "Type: " . $result[0]['alerttype'] . "\n";
        echo "Message: " . $result[0]['alertmessage'] . "\n";
        $time = $result[0]['alerttime'];
        
        echo "Time: " . date( 'd/m/Y h:i:s A', strtotime($time)) . "\n";
        
        $strSQL = "select unitName,lat,long,deviceID,CAST(velocity * 0.621 AS int) AS velocity," . 
                  "recTime,recTimeRevised from unitRecords where unitID = " . $unitID . 
                  " and recTimeRevised = '" . $time . "' order by recTime desc limit 1";
        $result = getResult($strSQL);
        if (count($result) > 0) {
            $lat = $result[0]["lat"];
            $lon = $result[0]["long"];
        
            echo "Speed: " . intval($result[0]['velocity']) . "\n";
            
            getReverseGeocoding($lat, $lon);
        } else {
            echo "This unit hasn't reported yet.\n";
        }
    } else {
        echo "No alerts for this unit.\n";
    }
    
    return 0;
}

function getHasAlert($data) {
    $unitID = $data['unitID'];
    $alertID = $data['alertID'];
    if ($unitID == null) {
        return -1;
    }

    header('Content-type: text/plain');
        
    if ($alertID == null || $alertID == -1) {
        $strSQL = "select * from tblAlert where unitID = " . $unitID . " and now() - alerttime < interval '5 minutes' order by alerttime desc limit 1;";
    } else {
        $strSQL = "select * from tblAlert where unitID = " . $unitID . " and now() - alerttime < interval '5 minutes' and id > " . $alertID . " order by alerttime desc limit 1;";
    }
    $result = getResult($strSQL);

    if (count($result) > 0) {
        echo $result[0]["id"];
    } else {
        echo "0";
    }
    
}

function getUserID($data) {
    // TODO: Return userID
    $username = $data['username'];
    if ($data['username'] == null || $data['password'] == null) {
        return -1;
    }
    
    $password = encrypt_des( $data['password']);
    
    $strSQL = "select * from tblUser where login = '" . $username . 
                                            "' and password = '" . $password . "';";
    $result = getResult($strSQL);

    header('Content-type: text/plain');
    
    if (count($result) < 1) {
        echo -1;
    }
    
    echo $result[0]['uid'];
    
    //return intval($result[0]['uid']);
}

function sendUnitCommand($data, $cmd) {
    $deviceID = $data['deviceID'];
    
    header('Content-type: text/plain');
    
    if ($deviceID == null) {
        echo -1;
        return;
    }
    
    $strSQL = "select fn_insert_unit_command('" . $cmd . "'," . $deviceID . ");";
    echo $strSQL . "\n";
    $result = getResult($strSQL);
    
    echo $result[0]['fn_insert_unit_command'];
}

function lock($data) {
    sendUnitCommand($data, "20,70");
}
function unlock($data) {
    sendUnitCommand($data, "20,20");
}
function silentAlarm($data) {
    sendUnitCommand($data, "20,69");
}
function panicAlarm($data) {
    sendUnitCommand($data, "20,31");
}
function stopEngine($data) {
    sendUnitCommand($data, "20,71");
}
function startEngine($data) {
    sendUnitCommand($data, "20,72");
}
function auxilary($data) {
    sendUnitCommand($data, "20,98");
}

function output_xml()
{
    header('Content-type: text/xml');
    echo "<?xml version=\"1.0\"?>";
    echo "\n";
}

function getUnits($data) {
    $userID = $data['userID'];
    
    if ($userID == null) {
        return -1;
    }
    
    $strSQL = "select distinct unitName from vwUserWiseUnits where uID = " . $userID . " and comID = (select comID from tblUser where uID = " . $userID . ");";
    $result = getResult($strSQL);
    
    output_xml();
    
    echo "<getUnits>";
    for ($i = 0; $i < count($result); $i++) {
        echo "<Unit>" . $result[$i]['unitname'] . "</Unit>" . "\n";
    }
    echo "</getUnits>";
    
    return 0;
}

/*function getUnits($data) {
    $userID = $data['userID'];
    
    if ($userID == null) {
        return -1;
    }
    
    $strSQL = "select unitID,unitName,deviceID from vwUserWiseUnits where uID = " . $userID;
    
    $result = getResult($strSQL);
    
    output_xml();
        
    echo "<getUnits>\n";
    for ($i = 0; $i < count($result); $i++) {
        echo "  <Unit>" . "\n";
        echo "    <DeviceID>" . $result[$i]['deviceid'] . "</DeviceID>" . "\n";
        echo "    <UnitID>" . $result[$i]['unitid'] . "</UnitID>" . "\n";
        echo "    <UnitName>" . $result[$i]['unitname'] . "</UnitName>" . "\n";
        echo "  </Unit>" . "\n\n";
    }
    echo "</getUnits>\n";

    return 0;
}*/

function getUnitInfo($data) {
    $unitName = $data['unitName'];
    $userID = $data['userID'];
    
    
    if ($unitName == null || $userID == null) {
        return -1;
    }
    
    $strSQL = "select unitID,unitName,deviceID from vwUserWiseUnits where unitName = '"
                                                . $unitName . "' and uID = " . $userID;
    
    $result = getResult($strSQL);
    
    output_xml();
    
    echo "<getUnitInfo>\n";
    for ($i = 0; $i < count($result); $i++) {
        //echo "  <Unit>" . "\n";
        echo "    <DeviceID>" . $result[$i]['deviceid'] . "</DeviceID>" . "\n";
        echo "    <UnitID>" . $result[$i]['unitid'] . "</UnitID>" . "\n";
        echo "    <UnitName>" . $result[$i]['unitname'] . "</UnitName>" . "\n";
        //echo "  </Unit>" . "\n\n";
    }
    echo "</getUnitInfo>\n";

    return 0;
}

function my_post() {
}

function my_put() {
} 

function my_delete() { }

function RunServer() {
    $callback = NULL;

    if (preg_match('/mobile\/([^\/]+)/', $_SERVER['REQUEST_URI'], $m)) {
        $splitStr = split("\?", $m[1]);
        if (isset($GLOBALS['RESTmap'][$_SERVER['REQUEST_METHOD']][$splitStr[0]])) {
            $callback = $GLOBALS['RESTmap'][$_SERVER['REQUEST_METHOD']][$splitStr[0]];
        }
    }

    if (is_callable($callback)) {
        $data = null;
        if ($_SERVER['REQUEST_METHOD'] == 'GET') {
            $data = $_GET;
        } else if ($_SERVER['REQUEST_METHOD'] == 'POST' || $_SERVER['REQUEST_METHOD'] == 'PUT') {
            $tmp = file_get_contents('php://input');
            parse_str($tmp, $data);
            //$data = json_decode($tmp);
        }

        header("{$_SERVER['SERVER_PROTOCOL']} 200 OK");
        //header('Content-Type: text/plain');
        //print json_encode(call_user_func($callback, $data));
        call_user_func($callback, $data);
    } else {
        header("{$_SERVER['SERVER_PROTOCOL']} 404 Not Found");

        exit;
    }
}

?>
