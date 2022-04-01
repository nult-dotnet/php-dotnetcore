<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Update"])){
        $data = array(
            "Name" => $_POST["Name"]
        );
        $request = "category/".$_POST["Id"];
        $headers = array("Content-Type: application/json");
        $result = CallAPI($request,"PUT",json_encode($data),$headers);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>