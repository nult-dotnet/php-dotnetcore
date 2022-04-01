<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        if(!empty($_FILES['file']['name'])){
            $data_array = array(
                "Name" => $_POST["name"],
                "Author" => $_POST["author"],
                "CategoryId" => $_POST["categoryId"],
                "Quantity" => $_POST["quantity"],
                "Price" => $_POST["price"],
                "File" => curl_file_create($_FILES['file']['tmp_name'], $_FILES['file']['type'], $_FILES['file']['name'])
            );
            $header = array("Content-Type: multipart/form-data");
            $result = CallAPI("book","POST",$data_array,$header);
            if(isset($result["Error"]) || isset($result["errors"])){
                print_r($result);
            }else{
                echo '<script>location.href = "index.php";</script>';
            }
        }
    }
?>