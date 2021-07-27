# Pages
## File page.cshtml.cs
- file page.cshtml.cs là model kế thừa class PageModel
- Có thể Inject qua constructor
- Có thể lấy các Request Parameter  trong Handler 
    - return RedirectToPage("page name")       
    - return Page()

    <!-- 
        public class ProductModel : PageModel
        {
            private readonly IProductContext _context;

            public Product product;
            public ProductModel(IProductContext context)
            {
                _context = context;
            }

            public IActionResult OnGet()
            {
                var id = Convert.ToInt32(Request.RouteValues["id"].ToString());
                product = _context.GetById(id);

                if(product == null){
                    return RedirectToPage("Products");
                }
                return Page();
            }
        } 
    -->

## File page.cshtml
- @page: có chỉ thị page
- @model: có chỉ thị model(Model tương ứng với ProductModel) là đối  tượng khai báo trong file page.cshtml.cs

    <!-- 
        @page "/Product/{id}"
        @model Web11.Pages.ProductModel 
    -->

## Data from Request
- Các lấy dữ liệu tự Request
    <!-- 
        var data1 = Request.RouteValues["data1"];               // Lấy dữ liệu từ tham số Route
        var data2 = Request.Query["data2"];                     // Lấy dữ liệu từ query url
        var data3 = Request.Form["data3"];                      // Lấy dữ liệu của Form gửi đến
        var data4 = Request.Form.Files["data4"];                // Lấy dữ liệu files
    -->


# ----------------------------------------------------------------
# Partial View
- Được tạo trong thư mục Pages/Shared và có _ để  phân biệt với các Page
- Không có chỉ thị @page đầu file cshtml
- File này chỉ có nhận mội đối tượng rồi Render ra html
    <!-- @model Web11.Models.Product -->

- Cách dùng
    <partial name="_ProductItem" model="item"/>

    <!-- 
        @await Html.PartialAsync("_ProductItem", item) 
    -->

    <!-- 
        @{
            await Html.RenderPartialAsync("_ProductItem", item)
        } 
    -->

# ----------------------------------------------------------------
# Component
- Giống Partial nhưng có 2 file
    - Một file render html
    - Một file có thể nhận Inject các service, nên dễ dàng xữ lý
- Được tạo trong Pages/Shared/Components/ProductBox/*
    - ProductBox.cs: tên file giống tên folder
        - Kế thừa từ ViewComponent
        - Có phương thức Invoke trả về <IViewComponentResult> hoặc string(ít khi)
        - Có thể nhận tham số từ hàm Invoke, khi gọi thì truyền vào TagHelper model
        - Có thể Inject service vào
        - Return về View(product): tức là trả về View mặc định là file Default.cshtml và có truyền đối tượng product vào Model
        
        <!-- 
            public class ProductBox : ViewComponent
            {
                private readonly IProductContext _context;
                public ProductBox(IProductContext context)
                {
                    
                }

                public IViewComponentResult Invoke(Product product)
                {
                    return View(product);
                }
            } 
        -->

    - Default.cshtml: mặc định tên file là Default.cshtml
        - Có chỉ thị @model để nhận giá trị từ phương thức Invoke truyền qua
    
        <!-- @model Web11.Models.Product -->

- Cách dùng:
    <!-- 
        @await Component.InvokeAsync("ProductBox", product); 
    -->

- Note: Trong Controller,các PageModel có thể trả về ngày ViewComponent bằng cách gọi
    - Như VD: set header như này sẽ tự động chuyển sang trang chủ trong vòng 3s sau khi vào Component ProductBox
    <!-- 
        this.HttpContext.Response.Headers.Add("REFRESH","3;URL=/");
        return ViewComponent("ProductBox", product); 
    -->



# ----------------------------------------------------------------

# ----------------------------------------------------------------

# ----------------------------------------------------------------

# ----------------------------------------------------------------

# ----------------------------------------------------------------

# ----------------------------------------------------------------

# ----------------------------------------------------------------

# ----------------------------------------------------------------