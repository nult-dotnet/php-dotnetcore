<?php
    function CallAPI($request,$type,$data, $headers = array("Content-Type: application/json")){
        $url ="https://localhost:44313/api/".$request;
        $ch = curl_init();
        $options = array(
            CURLOPT_HTTPHEADER => $headers,
            CURLOPT_URL => $url,
            CURLOPT_SSL_VERIFYPEER => false,
            CURLOPT_CUSTOMREQUEST => $type,
            CURLOPT_POSTFIELDS => $data,
            CURLOPT_RETURNTRANSFER => true
        );
        curl_setopt_array($ch,$options);
        $resp = curl_exec($ch);
        curl_close(($ch));
        $result = json_decode($resp,true);
        return $result;
    }
?>