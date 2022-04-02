<?php
    include_once __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Delete"])){
        $request = "user/".$_POST["Id"];
        $headers = array("Content-Type: application/json");
        $result = CallAPI($request,"DELETE","",$headers);
        echo '<script>location.href = "index.php";</script>';
    }
?>