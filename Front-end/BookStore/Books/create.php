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
                                Thêm mới sản phẩm
                            </div>
                            <div class="item_navbar">
                                <a href="">Trang chủ</a> / <a href="/../../php-dotnetcore/Front-end/BookStore/Books/">Quản lý sách</a> / <span>Thêm mới</span>
                            </div>
                        </div>
                        <div class="item-list_data">
                            <form action="add.php" method="POST" enctype="multipart/form-data">
                                <div class="form-group">
                                    <label for="name"><b>Tên sách</b></label>
                                    <input type="text" name="name"  id="name" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="file"><b>Hình ảnh</b></label>
                                    <input type="file" name="file" id="file" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="price"><b>Giá bán niêm yết</b></label>
                                    <input type="number" name="price" min="0" id="price" class="form-control">
                                </div>
                                 <div class="form-group">
                                    <label for="quantity"><b>Số lượng sách</b></label>
                                    <input type="number" name="quantity"  id="quantity" class="form-control">
                                </div>
                                <div class="form-group">
                                    <label for="author"><b>Nhà sản xuất</b></label>
                                    <input type="text"  name="author"  id="author" class="form-control">
                                </div>
                                <div class="form-group">
                                    <?php 
                                        include_once __DIR__ . "/../CallAPI.php";
                                        $data = CallAPI("category","GET","");
                                    ?>
                                    <label for="categoryId"><b>Phân loại sách</b></label>
                                    <select name="categoryId" id="categoryId" class="form-control">
                                        <option value=""></option>
                                        <?php foreach($data as $item): ?>
                                            <option value="<?=$item["id"]?>"><?=$item["name"]?></option>
                                        <?php endforeach;?>
                                    </select>
                                </div>
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