<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Delete"])){
        $resquest = "book/".$_POST["Id"];
        $headers = array("Content-Type: application/json");
        $result = CallAPI($resquest,"DELETE","",$headers);
        echo '<script>location.href = "index.php";</script>';
    }
?>