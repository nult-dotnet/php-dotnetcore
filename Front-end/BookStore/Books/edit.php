<?php
    include_once __DIR__ . '/../CallAPI.php';
    $request = "book/detail/".$_GET["id"];
    $data = CallAPI($request,"GET","");
    $dataBook = $data["object"];
?>
<?php if(isset($dataBook["Error"])):?>
<?php print_r($dataBook)?>
<?php else:?>
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
                                Cập nhật sản phẩm
                            </div>
                            <div class="item_navbar">
                                <a href="">Trang chủ</a> / <a href="/../../php-dotnetcore/Front-end/BookStore/Books/">Quản lý sách</a> / <span>Cập nhật</span>
                            </div>
                        </div>
                        <div class="item-list_data">
                            <form action="update.php" method="POST" enctype="multipart/form-data">
                                <div class="form-group">
                                    <label for=""><b>Mã sách</b></label>
                                    <input type="text" readonly name="id" value="<?=$dataBook["id"]?>" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="name"><b>Tên sách</b></label>
                                    <input type="text" value="<?=$dataBook["bookName"]?>" name="name"  id="name" class="form-control">
                                </div>
                                <div class="form-group">
                                    <div>
                                        <img src="https://localhost:44313/api/book/image/<?=$dataBook["imagePath"]?>" width="200px">
                                    </div>
                                    <label for="file"><b>Hình ảnh</b></label>
                                    <input type="file" name="file" id="file" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="price"><b>Giá bán niêm yết</b></label>
                                    <input type="text" value="<?=$dataBook["price"]?>" name="price"  id="price" class="form-control">
                                </div>
                                 <div class="form-group">
                                    <label for="quantity"><b>Số lượng sách</b></label>
                                    <input type="number" value="<?=$dataBook["quantity"]?>" name="quantity"  id="quantity" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="author"><b>Nhà sản xuất</b></label>
                                    <input type="text" value="<?=$dataBook["author"]?>" name="author"  id="author" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="categoryId"><b>Phân loại sách</b></label>
                                    <?php $data = CallAPI("category","GET","");?>
                                    <select name="categoryId" id="categoryId" class="form-control">
                                        <option value=""></option>
                                        <?php foreach($data as $item): ?>
                                            <?php if($dataBook["categoryId"] === $item["id"]): ?>
                                                <option selected value="<?=$item["id"]?>"><?=$item["name"]?></option>
                                            <?php else:?>
                                                <option value="<?=$item["id"]?>"><?=$item["name"]?></option>
                                            <?php endif;?>
                                        <?php endforeach;?>
                                    </select>
                                </div>
                                <div class="form-group">
                                    <label for=""><b>Thời gian cập nhật</b></label>
                                    <input type="text" readonly value="<?=date("H:i:s d-m-Y",strtotime($dataBook["timeCreate"]));?>" class="form-control">
                                </div>
                                <button type="submit" name="update" class="btn btn-primary">Cập nhật</button>
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