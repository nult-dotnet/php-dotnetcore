<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        $data_array = array(
            "Name" => $_POST["name"],
            "Author" => $_POST["author"],
            "CategoryId" => $_POST["categoryId"],
            "Quantity" => $_POST["quantity"],
            "Price" => $_POST["price"]
        );
        $result = CallAPI("book","POST",$data_array);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>