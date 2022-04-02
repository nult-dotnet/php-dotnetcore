<?php
    include_once __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Update"])){
        $request = "user/".$_POST["Id"];
        $data_array = array(
            "Name"=>$_POST["name"],
            "Email"=>$_POST["email"],
            "Phone"=>$_POST["phone"],
            "RoleId"=>$_POST["roleId"],
            "Address" => $_POST["address"]
        );
        $headers = array("Content-Type: application/json");
        $result = CallAPI($request,"PUT",json_encode($data_array),$headers);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>