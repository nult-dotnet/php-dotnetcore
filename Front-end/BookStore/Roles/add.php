<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        $data = array(
            "Name" => $_POST["Name"],
        );
        $result = CallAPI("role","POST",$data);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>