<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <?php include_once __DIR__ . "/../../php-dotnetcore/Front-end/BookStore/vendor/library.php" ?>
</head>
<body>
    <div style="margin: 50px;">
    <form action="addupload.php" method="post" enctype="multipart/form-data">
        <div class="form-group">
            <h2>Upload file large</h2>
            <b><label for="file">Upload file</label></b>
            <input type="file" name="file" id="file">
        </div>
        <button class="btn btn-primary" name="upload">Upload</button>
    </form>
    </div>
</body>
</html>