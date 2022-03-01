<?php
    include_once __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Delete"])){
        $request = "user/".$_POST["Id"];
        $result = CallAPI($request,"DELETE","");
        echo '<script>location.href = "index.php";</script>';
    }
?>