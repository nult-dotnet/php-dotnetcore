<?php
    include_once __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        $data_array = array(
            "BookId" => $_POST["Id"],
            "Quantity" => $_POST["Quantity"],
        );
        $result = CallAPI("bill","POST",$data_array);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>