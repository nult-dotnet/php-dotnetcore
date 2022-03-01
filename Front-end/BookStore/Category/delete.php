<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Delete"])){
        $request = "category/".$_POST["Id"];
        $result = CallAPI($request,"DELETE","");
        echo '<script>location.href = "index.php";</script>';
    }
?>