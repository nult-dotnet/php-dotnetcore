<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["update"])){
        $request = "book/detail/".$_POST["id"];
       
        $data_array = array(
            "Id" => $_POST["id"],
            "Name" => $_POST["name"],
            "Author" => $_POST["author"],
            "CategoryId" => $_POST["categoryId"],
            "Quantity" => $_POST["quantity"],
            "Price" => $_POST["price"]
        );
        $header = array("Content-Type: multipart/form-data");
        $result = CallAPI($request,"PUT",$data_array,$header);
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>