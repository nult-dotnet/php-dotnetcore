<?php
    include __DIR__ . '/../CallAPI.php';
    if(isset($_POST["Add"])){
        if(!empty($_FILES['file']['name'])){
            $data_file =  array (
                "fileName" => $_FILES['file']['name'],
                "length" => $_FILES['file']['size'],
                "tmp" => $_FILES['file']['tmp_name']
            );
            $data_array = array(
                "Name" => $_POST["name"],
                "Author" => $_POST["author"],
                "CategoryId" => $_POST["categoryId"],
                "Quantity" => $_POST["quantity"],
                "Price" => $_POST["price"],
                "File" => $data_file
            );
            $result = CallAPI("book","POST",$data_array);
            echo json_encode($result);
        }
    }
?>