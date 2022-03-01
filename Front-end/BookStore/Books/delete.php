<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Delete"])){
        $resquest = "book/".$_POST["Id"];
        $result = CallAPI($resquest,"DELETE","");
        echo '<script>location.href = "index.php";</script>';
    }
?>