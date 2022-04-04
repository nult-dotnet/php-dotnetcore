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
                            Danh mục phân loại sách
                        </div>
                        <div class="item_navbar">
                            <a href="">Trang chủ</a> / <a href="/../../php-dotnetcore/Front-end/BookStore/Category/">Phân loại sách</a>
                        </div>
                    </div>
                    <div class="insert_data text-right">
                        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal">Thêm loại sách</button>
                    </div>
                    <div class="modal_insert">
                        <div class="modal fade" id="myModal">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Thêm mới loại sách</h4>
                                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                                    </div>
                                        <form action="add.php" method="POST">
                                            <div class="modal-body">
                                                <div class="form-group">
                                                    <label for="Name"><b>Loại sách</b></label>
                                                    <input type="text" class="form-control" name="Name" id="Name">
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="submit" name="Add" class="btn btn-primary">Thêm</button>
                                                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="item-list_data">
                        <?php 
                            include_once __DIR__ . '/../CallAPI.php';
                            $data = CallAPI("category","GET","");
                        ?>
                        <table class="table">
                            <thead class="bg-dark">
                                <th class="text-center" width="10%">STT</th>
                                <th width="40%">Phân loại sách</th>
                                <th class="text-center">Số lượng loại sách</th>
                                <th class="text-center">Ngày cập nhật</th>
                                <th class="text-right">Hành động</th>
                            </thead>
                            <tbody>
                                <?php $i=0;?>
                                <?php foreach($data as $value): ?>
                                    <?php $i++;?>
                                    <tr>
                                        <td class="text-center"><b><?=$i?></b></td>
                                        <td><b><?=$value["name"]?></b></td>
                                        <td class="text-center"><?=$value["quantity"]?> </td>
                                        <td class="text-center"><?=date("H:i d-m-Y",strtotime($value["timeCreate"]))?></td>
                                        <td class="text-right" style="display: flex;column-gap: 5px;justify-content:flex-end">
                                            <a href="/../../php-dotnetcore/Front-end/BookStore/Category/edit.php?id=<?=$value["id"]?>" class="btn btn-info btn-sm"><i class="fas fa-edit"></i></a>
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