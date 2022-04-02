<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        $data = array(
            "Name" => $_POST["Name"]
        );
        $headers = array("Content-Type: application/json");
        $result = CallAPI("category","POST",json_encode($data),$headers);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>