<?php
    include_once __DIR__ . '/../CallAPI.php';
    $request = "category/".$_GET["id"];
    $data = CallAPI($request,"GET","");
?>
<?php if(isset($data["Error"])):?>
<?php print_r($data);?>
<?php else:?>
    <!DOCTYPE html>
    <html lang="en"> 
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>BookStore | Category</title>
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
                                Cập nhật loại sách
                            </div>
                            <div class="item_navbar">
                                <a href="">Trang chủ</a> / <a href="/../BookStore/Category/">Phân loại sách</a> / <span>Cập nhật</span>
                            </div>
                        </div>
                        <div class="item-list_data">
                            <form action="update.php" method="POST">
                                <div class="form-group">
                                    <label for=""><b>Mã phân loại sách</b></label>
                                    <input type="text" readonly name="Id" value="<?=$data["id"]?>" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="name"><b>Tên loại sách</b></label>
                                    <input type="text" value="<?=$data["name"]?>" name="Name"  id="name" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for=""><b>Thời gian cập nhật</b></label>
                                    <input type="text" readonly value="<?=date("H:i:s d-m-Y",strtotime($data["timeCreate"]))?>" class="form-control">
                                </div>
                                <button type="submit" name="Update" class="btn btn-primary">Cập nhật</button>
                            </form>
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