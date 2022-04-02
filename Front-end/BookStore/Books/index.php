<!DOCTYPE html>
<html lang="en"> 
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>BookStore | Books</title>
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
                            Danh mục quản lý sách
                        </div>
                        <div class="item_navbar">
                            <a href="">Trang chủ</a> / <a href="/../../php-dotnetcore/Front-end/BookStore/Books/">Quản lý sách</a>
                        </div>
                    </div>
                    <div class="insert_data text-right">
                        <a href="/../../php-dotnetcore/Front-end/BookStore/Books/create.php" class="btn btn-primary">Thêm mới</a>
                    </div>
                    <div class="item-list_data">
                        <?php 
                            include_once __DIR__ . '/../CallAPI.php';
                            $data = CallAPI("book","GET","");
                        ?>
                        <table class="table">
                            <thead class="bg-dark"> 
                                <th class="text-center" width="10%">STT</th>
                                <th width="10%">Hình ảnh</th>
                                <th>Tên sách</th>
                                <th class="">Giá bán niêm yết</th>
                                <th>Phân loại sách</th>
                                <th class="text-center">Trong kho</th>
                                <th class="text-right">Đã bán</th>
                                <th></th>
                            </thead>
                            <tbody>
                                <?php $i=0;?>
                                <?php foreach($data as $value): ?>
                                    <?php $i++;?>
                                    <tr>
                                        <td class="text-center"><b><?=$i?></b></td>
                                        <td><img src="https://localhost:44313/api/book/image/<?=$value["imagePath"]?>" width="100%"></td>
                                        <td><b><?=$value["bookName"]?></b></td>
                                       
                                        <td class=""><b class="text-success"><?=number_format($value["price"],0,',','.')?> VND</b></td>
                                        <td><?=$category = !empty($value["category"]["categoryName"]) ? $value["category"]["categoryName"]:"";?></td>
                                        <td class="text-center quantity"><b class="text-danger"><?=$value["quantity"] - $value["sold"]?></b> <i data-id="<?=$value["id"]?>" class="text-danger edit_quantity fas fa-pen-square"></i></td>
                                        <td class="text-center"><?=$value["sold"]?></td>
                                        <td class="text-right" style="display: flex;column-gap: 5px;justify-content:flex-end">
                                            <a href="/../../php-dotnetcore/Front-end/BookStore/Books/edit.php?id=<?=$value["id"]?>" class="btn btn-info btn-sm"><i class="fas fa-edit"></i></a>
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
                    <?php include_once __DIR__ . '/../../BookStore/layouts/partials/footer.php' ?>
                </div>
            </div>
        </div>
    </div>
</body>
<style>
    .quantity{
        cursor: pointer;
    }
    .edit_quantity{
        display: none;
    }
    .quantity:hover .edit_quantity{
        display: inline-block;
    }
</style>    
<script>
    $(document).ready(function(){
        $(".edit_quantity").click(function(){
            var quantity = $(this).parent().find("b").text();
            var id  = $(this).data("id");
            var formEdit = `
                <form action="updateQuantity.php" method="POST" style="display: flex;align-items: center;justify-content: center;column-gap:5px">
                    <input type="hidden" value="${id}" name="Id">
                    <input type="text" value="${quantity}" name="Quantity" style="width: 50px;">
                    <button name="Update" class="btn btn-primary btn-sm"><i class="fas fa-save"></i></button>
                </form>
            `;
            $(this).parent().html(formEdit);
        });
    });
</script>
</html>