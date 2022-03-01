<?php
    function CallAPI($request,$type,$data){
        $url ="https://localhost:44313/api/".$request;
        $ch = curl_init();
        curl_setopt($ch,CURLOPT_HTTPHEADER, array("Content-Type: application/json"));
        curl_setopt($ch,CURLOPT_URL,$url);
        curl_setopt($ch,CURLOPT_SSL_VERIFYPEER,false);
        curl_setopt($ch,CURLOPT_CUSTOMREQUEST,$type);
        if(!empty($data)){
            curl_setopt($ch,CURLOPT_POSTFIELDS,json_encode($data));
        }   
        curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
        $resp = curl_exec($ch);
        curl_close(($ch));
        $result = json_decode($resp,true);
        return $result;
    }
?>