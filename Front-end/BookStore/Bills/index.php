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
                            Danh mục quản lý hóa đơn
                        </div>
                        <div class="item_navbar">
                            <a href="">Trang chủ</a> / <a href="/../BookStore/Bills/">Quản lý hóa đơn</a>
                        </div>
                    </div>
                    <div class="insert_data text-right">
                        <a href="/../BookStore/Bills/create.php" class="btn btn-primary">Thêm mới</a>
                    </div>
                    <div class="item-list_data">
                        <?php 
                            include_once __DIR__ . '/../CallAPI.php';
                            $data  = CallAPI("bill","GET","");
                        ?>
                        <table class="table">
                            <thead class="bg-dark"> 
                                <th class="text-center" width="10%">STT</th>
                                <th width="30%">Mã hóa đơn</th>
                                <th class="">Tổng giá trị</th>
                                <th>Thời gian tạo hóa đơn</th>
                                <th class="text-center">Đơn vị tiền tệ</th>
                                <th></th>
                            </thead>
                            <tbody>
                                <?php $i=0;?>
                                <?php foreach($data as $value): ?>
                                    <?php $i++;?>
                                    <tr>
                                        <td class="text-center"><b><?=$i?></b></td>
                                        <td><b><?=$value["id"]?></b></td>
                                        <td class=""><b class="text-success"><?=number_format($value["value"],0,',','.')?> VND</b></td>
                                        <td><?=date("H:i:s d-m-Y",strtotime($value["timeCreate"]))?></td>
                                        <td class="text-center"><b><?=$value["currency"]?></b></td>
                                        <td class="text-right">
                                            <a href="/../BookStore/Bills/detail.php?id=<?=$value["id"]?>" class="btn btn-info btn-sm"><i class="fas fa-eye"></i></a>
                                        </td>
                                    </tr>
                                <?php endforeach?>
                            </tbody>
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