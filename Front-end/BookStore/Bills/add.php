<?php
    include_once __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        $data_array = array(
            "BookId" => $_POST["Id"],
            "Quantity" => $_POST["Quantity"],
        );
        $header = array("Content-Type: multipart/form-data");
        $result = CallAPI("bill","POST",json_encode($data_array));
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>