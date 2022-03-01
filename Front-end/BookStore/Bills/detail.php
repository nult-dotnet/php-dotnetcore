<?php
    $id = $_GET["id"];
    $ch = curl_init();
    $url = "https://localhost:44313/api/bill/".$id;
    curl_setopt($ch,CURLOPT_URL,$url);
    curl_setopt($ch,CURLOPT_SSL_VERIFYPEER,false);
    curl_setopt($ch,CURLOPT_RETURNTRANSFER,true);
    $response = curl_exec($ch);
    curl_close($ch);
    $dataBill = json_decode($response,true);
?>
<?php if(isset($dataBill["Error"])):?>
<?php print_r($dataBill)?>
<?php else:?>
    <!DOCTYPE html>
    <html lang="en"> 
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>BookStore | Bills</title>
        <?php include_once __DIR__ . '/../../BookStore/vendor/library.php'?>
    </head>
    <body>
        <div class="main">
            <div class="row no-gutters">
                <div class="col l-2 main_slidebar">
                    <div>
                        <?php include_once __DIR__ . '/../layouts/partials/slidebar.php' ?>
                    </div>
                </div>
                <div class="col l-10 main_content">
                    <div>
                        <?php include_once __DIR__ . '/../layouts/partials/header.php' ?>
                    </div>
                    <div class="item_content">
                        <div class="item_header">
                            <div class="item_title">
                                Chi tiết hóa đơn bán hàng
                            </div>
                            <div class="item_navbar">
                                <a href="">Trang chủ</a> / <a href="/../BookStore/Bills/">Quản lý hóa đơn</a> / <span>Chi tiết hóa đơn</span>
                            </div>
                        </div>
                        <div class="item-list_data">
                            <table class="table">
                                <tr>
                                    <td><b>Mã hóa đơn</b></td>
                                    <td><b><?=$dataBill["id"]?></b></td>
                                </tr>
                                <tr>
                                    <td><b>Giá trị hóa đơn</b></td>
                                    <td><b class="text-success"><?=number_format($dataBill["value"],0,",",".")?> VND</b></td>
                                </tr>
                                <tr>
                                    <td><b>Chi tiết hóa đơn</b></td>
                                    <td>    
                                        <table class="table table-borderless">
                                            <thead >
                                                <th style="color: black;" width="50%">Tên sản phẩm</th>
                                                <th style="color: black;">Giá bán</th>
                                                <th style="color: black;">Số lượng mua</th>
                                                <th style="color: black;">Tạm tính</th>
                                            </thead>
                                            <?php foreach($dataBill["books"] as $item): ?>
                                                <tbody>
                                                    <tr>
                                                        <td><b><?=$item["name"]?></b></td>
                                                        <td><?=number_format($item["price"],0,",",".")?> VND</td>
                                                        <td><?=$item["quantity"]?> sản phẩm</td>
                                                        <td><b class="text-success"><?=number_format($item["price"]*$item["quantity"],0,",",".")?> VND</b></td>
                                                    </tr>
                                                </tbody>
                                            <?php endforeach;?>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Thời gian tạo hóa đơn</b></td>
                                    <td><?=date("H:i:s d-m-Y",strtotime($dataBill["timeCreate"]))?></td>
                                </tr>
                                <tr>
                                    <td><b>Đơn vị tiền tệ</b></td>
                                    <td><b><?=$dataBill["currency"]?></b></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div>
                        <?php include_once __DIR__ . '/../layouts/partials/footer.php' ?>
                    </div>
                </div>
            </div>
        </div>
    </body>
    </html>
<?php endif;?>