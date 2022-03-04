<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Delete"])){
        $request = "category/".$_POST["Id"];
        $headers = array("Content-Type: application/json");
        $result = CallAPI($request,"DELETE","",$headers);
        echo '<script>location.href = "index.php";</script>';
    }
?>