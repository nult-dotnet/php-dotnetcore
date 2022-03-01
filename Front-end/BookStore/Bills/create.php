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
                                Tạo hóa đơn bán hàng
                            </div>
                            <div class="item_navbar">
                                <a href="">Trang chủ</a> / <a href="/../BookStore/Bills/">Quản lý hóa đơn</a> / <span>Thêm mới</span>
                            </div>
                        </div>
                        <div style="display: flex;margin-top: 40px;column-gap: 25px;">
                            <?php 
                                include_once __DIR__ . "/../CallAPI.php";
                                $data = CallAPI("book","GET","");
                            ?>
                            <div>
                                <select name="selectBook" id="selectBook" class="form-control">
                                    <option value=""></option>
                                    <?php foreach($data as $bookItem):?>
                                        <option value="<?=$bookItem["id"]?>" data-price="<?=$bookItem["price"]?>"><?=$bookItem["bookName"]?></option>
                                    <?php endforeach;?>
                                </select>
                            </div>
                            <div>
                                <input type="number" id="quantity" min="1" class="form-control">
                            </div>
                            <div>
                                <button class="btn btn-primary btnAdd">Add</button>
                            </div>
                        </div>
                        <div class="item-list_data">
                            <form action="add.php" method="POST">
                                <table class="table">
                                    <thead>
                                        <th width="40%" style="color: black;">Tên sách</th>
                                        <th style="color: black;">Giá bán</th>
                                        <th class="text-center" style="color: black;">Số lượng mua</th>
                                        <th class="text-center" style="color: black;">Đơn vị tiền tệ</th>
                                    </thead>
                                    <tbody id="addItem">
                                       
                                    </tbody>
                                </table>
                                <button type="submit" name="Add" class="btn btn-primary">Thêm mới</button>
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
<script>
    $(document).ready(function(){
        $(".btnAdd").click(function(){
            var bookName = $("#selectBook").find("option:selected").text();
            var quantity = $("#quantity").val();
            var id = $("#selectBook").find("option:selected").val();
            var price = $("#selectBook").find("option:selected").data("price");
            var add = `
                <tr>
                    <td>
                        ${bookName}
                        <input type="hidden" name="Id[]" value="${id}">
                    </td>
                    <td>
                        ${price}
                    </td>
                    <td class="text-center">
                        ${quantity}
                        <input type="hidden" name="Quantity[]" value="${quantity}">
                    </td>
                    <td class="text-center">
                        VND
                    </td>
                </tr>
            `;
            $("#addItem").append(add);
            $("#quantity").val("");
            $("#selectBook").prop("value",'');
        });
    });
</script>