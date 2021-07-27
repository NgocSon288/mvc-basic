# 1. MVC
- Lệnh giúp làm việc với mô hình MVC
    <!-- services.AddControllersWithViews(); -->

# ------------------------------------------------------------
# 2. Static files
- Lệnh chỉ ra thư mục wwwroot(Mặc định) làm thư mục static
    <!-- app.UseStaticFiles(); -->

# ------------------------------------------------------------
# 3. Routing && Endpoints
- Lệnh để giúp tạo ra các routing và dùng các controllers để routing 
    <!-- 
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }); 
    -->

# ------------------------------------------------------------
# 4. _ViewStart
- Chứa code chung của các files, folders cùng cấp(Gôm lại): các code như layout 

# ------------------------------------------------------------
# 5. _ViewImport
- Chứa các imports, inject.. chung của files, folders cùng cấp

# ------------------------------------------------------------
# 6. Routing
- Các tham số routing 
    <!-- MapControllerRoute(string name, string pattern, object defaults = null, object constraints = null); -->
    - [name] tên của route. 
    - [pattern] mẫu URL. 
    - [defaults] giá trị mặc định cho route. 
    - [constraints] là các ràng buộc
        - :int
        - :min(1)
        - :alpha
        - :minlength(5)


# ------------------------------------------------------------
# 7. Form Tag Helper
- khi dùng asp-for/asp-validation-for
    - [label]: thể hiện ra DisplayName
    - [input]: thể hiện ra value
    - [span]: thể hiện ra lỗi

# ------------------------------------------------------------
# 8. Tạo SelectList
- Tạo ra SelectList bên Controller. Với [items] là List<>, [ID] là thuộc tính giá trị của option, [Name] là text hiển thị, 3 là giá trị được chọn khởi tạo 
    <!-- ViewBag.items = new SelectList(items, "ID", "Name", 3); -->

- Sử dụng bên View
    <!-- <select class="form-control" asp-for="Language" asp-items="@ViewBag.items"></select> -->

## Tạo group bằng SelectListItem
- 
    <!-- 
        var group1 = new SelectListGroup() { Name = "Group 1", Disabled = false };
        var group2 = new SelectListGroup() { Name = "Group 2", Disabled = false };
        var group3 = new SelectListGroup() { Name = "Group 3", Disabled = false };

        var items =  new List<SelectListItem>()
        {
            new SelectListItem(){Value = "1", Text = "Tiếng Anh",  Group = group1},
            new SelectListItem(){Value = "2", Text = "Tiếng Việt",  Group = group1},
            new SelectListItem(){Value = "3", Text = "Tiếng Hàn",  Group = group2},
            new SelectListItem(){Value = "4", Text = "Tiếng Trung",  Group = group2},
            new SelectListItem(){Value = "5", Text = "Tiếng Ý",  Group = group3},
            new SelectListItem(){Value = "6", Text = "Tiếng Hà Lan",  Group = group3},
        };

        ViewBag.items = items;
    -->

## Hoặc dùng Enum

# ------------------------------------------------------------
# 9. Sử dụng Client site validation
- Thêm các thư viện LibMan
    <!-- 
        jquery-validate@1.19.3
        jquery-validation-unobtrusive@3.2.12 
    -->

- Sau đó thêm 3 file trong 2 thư mục vừa tải về
    <!-- 
        <script src="~/jquery-validate/additional-methods.js"></script>
        <script src="~/jquery-validate/jquery.validate.js"></script>
        <script src="~/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js"></script>
     -->

# ------------------------------------------------------------
# 10. Thiết lập IOption
- Có thể dùng để Configure Email... 
    <!-- 
        "test": {
            "ID": 100,
            "Name": "Huỳnh Ngọc Sơn",
            "Age": 21
        }, 
    -->

- Tạo lớp với các thuộc tính tương ứng với key
- Đăng ký service trong ConfigureService
    <!-- services.Configure<Person>("key", _configuration.GetSection("test")); -->

- Inject và sử dụng([IOptionsMonitor])
    <!-- 
        public HomeController(IOptionsMonitor<Person> option)
        {
            _person = option.CurrentValue;
        } 
    -->

- Inject và sử dụng([IOptions])
    <!-- 
        public HomeController(IOptions<Person> option)
        {
            _person = option.Value;
        } 
    -->

- Inject và sử dụng trong trường hợp 2 section cùng chung một class
     <!-- 
        public HomeController(IOptions<Person> option)
        {
            _person = option.Get("Key")
        } 
    -->

## Note option
                    Singleton               Reloading           Named Options
IOptions            yes                     no                  no
IOptionsSnapshot    no                      yes                 yes
IOptionsMonitor     yes                     yes                 yes

# ------------------------------------------------------------
# 11. Return URL
- Bên form login chỉ cần [method=post] là xong, để cho asp tự động vì nếu thêm action vào là nó gán lại url khác
    <!-- <form method="post"> -->

- Bên [Action] thì thêm tham số nhận URL
    <!-- public async Task<IActionResult> Login(SigninModel model, [FromQuery]string returnUrl) -->

- Và sau đó dùng [LocalRedirect] để chuyển hướng trang đến returlUrl
    <!-- return LocalRedirect(returnUrl); -->


# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 

# ------------------------------------------------------------
# 