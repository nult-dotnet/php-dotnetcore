<!DOCTYPE html>
<html lang="en"> 
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>BookStore | Staff</title>
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
                            Danh mục quản lý nhân viên
                        </div>
                        <div class="item_navbar">
                            <a href="">Trang chủ</a> / <a href="/../../php-dotnetcore/Front-end/BookStore/Staffs/">Quản lý nhân viên</a>
                        </div>
                    </div>
                    <div class="insert_data text-right">
                        <a href="/../../php-dotnetcore/Front-end/BookStore/Staffs/create.php" class="btn btn-primary">Thêm mới</a>
                    </div>
                    <div class="item-list_data">
                        <?php 
                            include_once __DIR__ . '/../CallAPI.php';
                            $data = CallAPI("user","GET","");
                        ?>
                        <table class="table">
                            <thead class="bg-dark"> 
                                <th class="text-center" width="10%">STT</th>
                                <th width="20%">Nhân viên</th>
                                <th class="">Email</th>
                                <th>Số điện thoại</th>
                                <th class="">Chức vụ nhân viên</th>
                                <th></th>
                            </thead>
                            <tbody>
                                <?php $i=0;?>
                                <?php foreach($data as $value): ?>
                                    <?php $i++;?>
                                    <tr>
                                        <td class="text-center"><b><?=$i?></b></td>
                                        <td><b><?=$value["name"]?></b></td>
                                        <td class=""><?=$value["email"]?></td>
                                        <td class=""><?=$value["phone"]?></td>
                                        <td class="">
                                            <?=$role = !empty($value["role"]["name"]) ? $value["role"]["name"] : "";?>
                                        </td>
                                        <td class="text-right" style="display: flex;column-gap: 5px;justify-content:flex-end">
                                            <a href="/../../php-dotnetcore/Front-end/BookStore/Staffs/edit.php?id=<?=$value["id"]?>" class="btn btn-info btn-sm"><i class="fas fa-edit"></i></a>
                                            <form action="delete.php" method="POST">
                                                <input type="hidden" name="Id" id="id" readonly value="<?=$value['id']?>"/>
                                                <button class="btn btn-danger btn-sm" type="submit" name="Delete"><i class="fas fa-trash-alt"></i></button>
                                            </form>  
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