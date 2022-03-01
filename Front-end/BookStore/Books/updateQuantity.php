<?php
    include_once __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Update"])){
        $resquest = "book/".$_POST["Id"];
        $data =  array(
            ["op" => "replace","path" =>  "/Quantity","value" => $_POST["Quantity"]]
        );
        $result = CallAPI($resquest,"PATCH",$data);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>