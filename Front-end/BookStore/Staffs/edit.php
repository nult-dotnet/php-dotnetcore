<?php
    include_once __DIR__ . '/../CallAPI.php';
    $request = "user/".$_GET["id"];
    $user = CallAPI($request,"GET","");
?>
<?php if(isset($user["Error"])):?>
<?php print_r($user)?>
<?php else:?>

<!DOCTYPE html>
    <html lang="en"> 
    <head>
        <meta charset="UTF-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>BookStore | Staffs</title>
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
                                Cập nhật thông tin nhân viên
                            </div>
                            <div class="item_navbar">
                                <a href="">Trang chủ</a> / <a href="/../../php-dotnetcore/Front-end/BookStore/Staffs/">Quản lý nhân viên</a> / <span>Cập nhật thông tin</span>
                            </div>
                        </div>
                        <div class="item-list_data">
                            <form action="update.php" method="POST">
                                <div class="form-group">
                                    <label for="Id"><b>Tên nhân viên</b></label>
                                    <input type="text" name="Id"  id="Id" readonly value="<?=$user["id"]?>" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="name"><b>Tên nhân viên</b></label>
                                    <input type="text" name="name"  id="name" value="<?=$user["name"]?>" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="email"><b>Địa chỉ email</b></label>
                                    <input type="email" readonly name="email" id="email" value="<?=$user["email"]?>" class="form-control">
                                </div>
                                 <div class="form-group">
                                    <label for="phone"><b>Số điện thoại</b></label>
                                    <input type="tel" name="phone" value="<?=$user["phone"]?>" id="phone" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="address"><b>Địa chỉ</b></label>
                                    <input type="text"  name="address" value="<?=$user["address"]?>" id="address" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="role"><b>Chức vụ</b></label>
                                    <?php $data = CallAPI("role","GET","");?>
                                    <select name="roleId" id="role" class="form-control">
                                        <option value=""></option>
                                        <?php foreach($data as $val):?>
                                            <?php if($val["id"] === $user["roleId"]): ?>
                                                <option selected value="<?=$val["id"]?>"><?=$val["name"]?></option>
                                            <?php else:?>
                                                <option value="<?=$val["id"]?>"><?=$val["name"]?></option>
                                            <?php endif;?>
                                        <?php endforeach;?>
                                    </select>
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