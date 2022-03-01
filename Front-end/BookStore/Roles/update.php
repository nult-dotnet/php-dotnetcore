<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Update"])){
        $request = "role/".$_POST["Id"];
        $data = array(
            "Name" => $_POST["Name"]
        );
        $result = CallAPI($request,"PUT",$data);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>