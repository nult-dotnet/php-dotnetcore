<?php
    include_once __DIR__ . '/../../php-dotnetcore/Front-end/BookStore/CallAPI.php';
    ini_set('memory_limit', '1024M');
    function generateRandomString($length = 10) {
        $characters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
        $charactersLength = strlen($characters);
        $randomString = '';
        for ($i = 0; $i < $length; $i++) {
            $randomString .= $characters[rand(0, $charactersLength - 1)];
        }
        return $randomString;
    }
    if(isset($_POST["upload"])){
        if($_FILES["file"]["name"]){
            $offset = 0;
            $file_size = $_FILES["file"]["size"];
            $file_path = $_FILES["file"]["tmp_name"];
            $chunk_size = ($file_size > 536870912) ? 536870912 : $file_size; //500M;
            $header = array("Content-Type: multipart/form-data");
            $request = "book/upload";
            $i=0;
            $data = [];
            $ext = pathinfo($_FILES["file"]["name"], PATHINFO_EXTENSION);
            $filename = generateRandomString();
            while($offset < $file_size){
                if($offset + $chunk_size >= $file_size){
                    $sz = $file_size - $offset;
                }else{
                    $sz = $chunk_size;
                }
                $file = $filename.'.'.$ext.$i;
                $chunkFile = file_get_contents($file_path,false,null,$offset,$sz);
                file_put_contents($file,$chunkFile);
                $offset += $chunk_size;
                $i++;
                $data["file"] = curl_file_create("D:/xampp/htdocs/php-dotnetcore/Front-end/".$file,mime_content_type($file),$file);
                $result = CallAPI($request,"POST",$data,$header);
                unlink($file);
                // echo json_encode($result);
            }
            $request_merge = "book/filemerge/".$filename.'.'.$ext;
            $mergeFile = CallAPI($request_merge,"POST",$filename.'.'.$ext);
            if(isset($mergeFile["Error"]) || isset($mergeFile["errors"])){
                print_r($mergeFile);
            }else{
                echo 'Upload success';
            }
        }  
    }
?>