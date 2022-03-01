<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["update"])){
        $request = "book/detail/".$_POST["id"];
       
        for($i=1;$i<=2;$i++){
            $data_array = array(
                "Id" => $_POST["id"],
                "Name" => $_POST["name"].$i,
                "Author" => $_POST["author"],
                "CategoryId" => $_POST["categoryId"],
                "Quantity" => $_POST["quantity"],
                "Price" => $_POST["price"]
            );
            $result = CallAPI($request,"POST",$data_array);
        }
        if(isset($result["Error"]) || isset($result["errors"])){
            print_r($result);
        }else{
            echo '<script>location.href = "index.php";</script>';
        }
    }
?>