<?php
    include_once __DIR__ . '/../../php-dotnetcore/Front-end/BookStore/CallAPI.php';
    ini_set('memory_limit', '1024M');
    function generateRandomString($length = 15) {
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
            $file_name = $_FILES["file"]["name"];
            $chunk_size = ($file_size > 104857600) ? 104857600 : $file_size; //100M;
            $header = array("Content-Type: multipart/form-data");
            $url = "upload";
            $totalChunk = (($file_size - ($file_size % $chunk_size)) / $chunk_size);
            if(($file_size % $chunk_size) != 0){
                $totalChunk++;
            }
            $ext = pathinfo($_FILES["file"]["name"], PATHINFO_EXTENSION);
            $fileId = generateRandomString();
            for($i=1;$i<=$totalChunk;$i++){
                $data = [];
                if($offset + $chunk_size >= $file_size){
                    $sz = $file_size - $offset;
                }else{
                    $sz = $chunk_size;
                }
                $file = "chunk".$i.".blob";
                $chunkFile = file_put_contents($file,file_get_contents($file_path,false,null,$offset,$sz));//Blob file
                $offset += $chunk_size;
                $data = array(
                    $file => curl_file_create("D:/xampp/htdocs/php-dotnetcore/Front-end/".$file,mime_content_type($file),$file),
                    "chunkNumber" => $i,
                    "totalChunks" => $totalChunk,
                    "fileName" => $file_name,
                    "fileId" => $fileId
                );
                $result = CallAPI($url,"POST",$data,$header);
                unlink($file);
                if(isset($result["Error"]) || isset($result["errors"])){
                    print_r($result);
                }else{
                    if($result["complete"]==true){
                        echo 'Upload complete';
                    }
                }
            }
        }  
    }
?>