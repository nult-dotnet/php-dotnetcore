<link rel="stylesheet" href="/../../php-dotnetcore/Front-end/BookStore/layouts/css/style_slidebar.css">
<script src="/../../php-dotnetcore/Front-end/BookStore/vendor/jquery/jquery.min.js"></script>

<div class="slidebar_container">
    <a class="slidebar_header">
        <img class="logo">
        <span class="title">ASP.NET Core API</span>
    </a>
    <div class="slidebar_content">
        <div class="slidebar_user">
            <i class="fas fa-user-circle"></i> <span class="text">Nguyễn Công Thiện</span> 
        </div>
        <div class="slidebar_menu">
            <label class="slider_menu--header"  for="check_show_menu">
                <i class="fas fa-folder nav-icon"></i><span class="">Danh mục quản lí</span>
            </label>
            <input type="checkbox" id="check_show_menu" checked hidden>
            <div id="menu_container">
                <div id="menu"> 
                    <ul>
                        <li class="slidebar_menu--item">
                            <a href="/../../php-dotnetcore/Front-end/BookStore/Books/">
                                <i class="nav-icon fas fa-th"></i><span class="link">Quản lý sách</span>
                            </a>
                        </li>
                        <li class="slidebar_menu--item">
                            <a href="/../../php-dotnetcore/Front-end/BookStore/Category/">
                                <i class="nav-icon fas fa-th"></i><span class="link">Phân loại sách</span>
                            </a>
                        </li>
                        <li class="slidebar_menu--item">
                            <a href="/../../php-dotnetcore/Front-end/BookStore/Bills/">
                                <i class="nav-icon fas fa-th"></i><span class="link">Quản lý hóa đơn</span>
                            </a>
                        </li>
                        <li class="slidebar_menu--item">
                            <a href="/../../php-dotnetcore/Front-end/BookStore/Staffs/">
                                <i class="nav-icon fas fa-th"></i><span class="link">Quản lý nhân viên</span>
                            </a>  
                        </li><li class="slidebar_menu--item">
                            <a href="/../../php-dotnetcore/Front-end/BookStore/Roles/">
                                <i class="nav-icon fas fa-th"></i><span class="link">Vai trò nhân viên</span>
                            </a>  
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
