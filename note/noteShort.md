# Xác thức https 
    dotnet dev-certs https --clean
    dotnet dev-certs https --trust

# ------------------------------------------------------------
# app.UseRouting(): Endpoint Routing Middleware
- Phân tích địa chỉ truy cập
    app.UseRouting()
- Tạo các EndPoint
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Xin chào ASP.NET Core");
        });
    });

# ------------------------------------------------------------
# app.UseStatusCodePages(): Status Code MiddleWare
- Nếu không có địa chỉ nào phụ hợp nó sẽ vào đó
- Có thể đổi thư mục khác
    webBuilder.UseWebRoot("public");
    
# ------------------------------------------------------------
# app.UseStaticFiles(): Static File MiddleWare
- Dùng thư mục wwwroot làm mặc định

# ------------------------------------------------------------
# Install FrameWork FE
- Tạo thư mục src chứa các scss
- Tạo Webpack để đóng gói
    npm init -y                                         # tạo file package.json cho dự án
    npm i -D webpack webpack-cli                        # cài đặt Webpack
    npm i node-sass postcss-loader postcss-preset-env   # cài đặt các gói để làm việc với SCSS
    npm i sass-loader css-loader cssnano                # cài đặt các gói để làm việc với SCSS, CSS
    npm i mini-css-extract-plugin cross-env file-loader # cài đặt các gói để làm việc với SCSS
    npm install copy-webpack-plugin                     # cài đặt plugin copy file cho Webpack
    npm install npm-watch                               # package giám sát file  thay đổi

- Cài đặt các thư viện
    npm install bootstrap                               # cài đặt thư viện bootstrap
    npm install jquery                                  # cài đặt Jquery
    npm install popper.js                               # thư viện cần cho bootstrap

- Tạo file webpack.config.js    
 
- Thêm vào package.json 
    "watch": {
        "build": "src/scss/site.scss"
      },
      "scripts": {
        "test": "echo \"Error: no test specified\" && exit 1", 
        "build": "webpack",
        "watch": "npm-watch"
      },
- Thực thi
    npm run build
    npm run watch

- Lệnh import scss
    @import './../../node_modules/bootstrap/scss/bootstrap.scss';

# ------------------------------------------------------------
# Dùng BuilderBundlerMinifler
- Cái đặt Package
    dotnet add package BuildBundlerMinifier
- Tạo file
    bundleconfig.json
- Nội dung
    {
        {
            "outputFileName": "wwwroot/css/bootstrap.min.css",
            "inputFiles": [
                "node_modules/bootstrap/dist/css/bootstrap.min.css"
            ]
        }
    }

# ------------------------------------------------------------
# Đăng ký bất kỳ class nào cũng là Service
- Tạo class ABC
- Đăng ký class đó  với services trong ConfigureService
- Giờ là có thể dùng, Inject vào bất kỳ đối tượng nào

# ------------------------------------------------------------
# Tạo custom Middleware    
- Tạo class FirstMiddleware được triển khai từ Interface IMiddleware
- Implement phương thức InvokeAsync
- Đăng ký dịch vụ với trong ConfigureService
    services.AddSingleton<FirstMiddleware>();
- Tạo lớp ExtensionMiddleware chứa các phương thức mở rộng của IApplicationBuilder và tạo phương thức mở rộng cho FirstMiddleware đó
    public static void UseFirstMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<FirstMiddleware>();
    }
- Dùng trong Configure
    app.UseFirstMiddleware();

# ------------------------------------------------------------
# Rẽ nhành
- Rẽ thành 1 nhánh con, chức năng và cấu trúc như nhanh cha
     app.Map("/admin", (app1)=>{
         // app.UseMiddleware<>;
         // app.UseRouting();
         // app.UseEndpoints();
     }

# ------------------------------------------------------------
# các thuộc  tính của Request
- khai báo request = context.Request;
    request.Scheme; (http, https)
    request.Host.Value
    request.Path
    request.QueryString.Value
    request.Method
    request.Headers
    request.Cookies
    request.Query   

# ------------------------------------------------------------
# Gửi Json
- Cài đặt Json
    dotnet add package Newtonsoft.Json [--version 13.0.1]
- Dùng
    context.Response.ContentType = "application/json";
    var json = JsonConvert.SerializeObject(p);

# ------------------------------------------------------------
# Lấy giá trị từ URL
    endpoints.MapGet("/Cookies/{*action}", async (context) =>
    {
        // Get giá trị trên url đã định nghĩa sẵng, nếu không có thì trả về null
        var action = context.GetRouteValue("action");
    });

- {*action} có nghĩa là có cũng được, không cũng được

# ------------------------------------------------------------
# Tạo cookies
- Khởi tạo cookies có option
    var option = new CookieOptions(){
        Path = "/",
        Expires = DateTime.Now.AddDays(1)
    };
- Tạo cookie
    context.Response.Cookies.Append("masanpham", "1231231231231231", option);

# ------------------------------------------------------------
# Forms
- Lấy ra form
    form = request.Form;
- Lấy dữ liệu trên form, trên các input
    email = form["email"].FirstOrDefault() ?? "";
    hovaten = form["hovaten"].FirstOrDefault() ?? "";

- Lấy dữ liệu liên quan đến file

    // var filePath = Path.GetTempFileName();
    // Xử lý nếu có file upload (hình ảnh,  ... )
    if (!Directory.Exists("wwwroot/upload/"))  Directory.CreateDirectory("wwwroot/upload/");
    if (form.Files.Count > 0) {
        string thongbaofile = "Các file đã upload: ";
        foreach (IFormFile formFile in _form.Files)
        {
            if (formFile.Length > 0)
            {
                var filePath = "wwwroot/upload/"+formFile.FileName;    // Lấy tên  file
                thongbaofile += $"{filePath} {formFile.Length} bytes";
                using (var stream = new FileStream(filePath, FileMode.Create)) // Mở stream để lưu file, lưu file ở thư mục wwwroot/upload/
                {
                        await formFile.CopyToAsync(stream);
                }
            }

        }
        thongbao += "<br>" + thongbaofile;
    }

# ------------------------------------------------------------
# Session
- Cài đặt Package
    dotnet add package Microsoft.AspNetCore.Session
    dotnet add package Microsoft.Extensions.Caching.Memory

- Đăng ký service trong ConfigureService
    services.AddDistributedMemoryCache();

- Cấu hình Session trong ConfigureService
    service.AddSession(option=>{
        option.Cookie.Name = "cookie-name";
        option.IdleTimeout = new TimeSpan(0,1,0);
    });

- Thêm vào đầu pipeline để khi dữ liệu đi qua thì Session sẽ được phục hồi
    app.UseSessions();

- Truy vấn
    context.Session;

- Các phương thức có thể dùng trong Session
    ID
    Clear()
    
    Set(key, value)         // value là byte[]
    TryGetValue(key, value) // lấy ra byte[] đưa vào value(return false là không thành công)

    SetString(key, value)   // value là string
    GetString(key)
    
    SetInt32(key, value)    // value là int
    GetInt32(key)

    
# ------------------------------------------------------------
# Tạo và dùng Options cơ bản
- Tạo class MyOptions chứa các thuộc tính
- Kích hoạt option của nó trong ConfigureService
    services.AddOptions();

- Đăng ký Option như một service trong ConfigureService nhưng sẽ được IOptions quản lý
    services.Configure<MyOptions>((MyOptions option)=>{
        option.val1= "1";
        option.val2= "2";
    });

- Có thể Inject vào các đối tượng khác để dùng
    public FirstMiddleware(GetData getdata, IOptions<MyOptions> options)
    {
        _getdata = getdata;
        _options = options.Value;   //   lấy ra value đã đăng ký,  class tương ứng
        // _options = options.Get("name"); // nếu như trùng class convert
    } 

# ------------------------------------------------------------
# Tạo và dùng Options được khởi tạo giá trị từ một file (appsettings.json)
- Tạo class MyOptions

- Kích hoạt Options trong ConfigureService
    services.AddOptions();

- Inject đối tượng IConfiguration vào class Startup, đối tượng này giúp lấy ra các Section trong appsettings.json
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration  = configuration;
    }

- Get ra Section trong appsettings.json ra thông qua đối tượng _configuration mà ta ta muốn dùng nó làm tham số khởi tạo cho MyOptions(Section lấy ra được khởi tạo trong appsettings.json, có các key tương đồng như các thuộc tính của class MyOptions)
    var testOptions = _configuration.GetSection("TestOption");  // TestOption là key trong appsettings.json

- Đăng ký Option như một service, nhưng thay vì khởi tạo bằng cách trực tiếp, ta khởi tạo bằng tham số từ   appsettings.json
    services.Configure<MyOptions>(testOptions);
    // services.Configure<MyOptions>("name", testOptions); // nếu như trùng class convert

- Giờ thì ta có thể Inject vào các đối tượng khác bằng IOptions<MyOption>,  phải thông qua IOptions, vì mấy Service loại options này đó IOptions quản lý

- Hoặc tự lấy context.RequestServices (đối tượng IServiceProvider)
    var myOption = context.RequestServices.GetService<IOptions<MyOptions>>().Value;

## Note option
                    Singleton               Reloading           Named Options
IOptions            yes                     no                  no
IOptionsSnapshot    no                      yes                 yes
IOptionsMonitor     yes                     yes                 yes

# ------------------------------------------------------------
# Partial View
- Giúp Chia nhỏ file page thành nhiều file
- Giúp tài sử dụng ở nhiều trang khác nhau
- Khi tạo Partial thì không có chỉ thị @page trên đầu trang

- Chèn Partial vào page
    <partial  name="_ProductItem" />

- Cách 2 để chèn
    @await Html.PartialAsync("_ProductItem")

- Cách 3: chèn vào vị trí code c#
    @{
        await Html.RenderPartialAsync("_ProductItem")
    }

# ----------------------------------------------------------------
# Truyền dữ liệu vào Partial
- Ta cần tạo Model class ABC  cho nó
- Vào truyền đối tượng của model class ABC đó qua TagHelper model (abc là đối tượng của lớp ABC)
    <partial name="_ProductItem" model="abc"/>
Hoặc
    @await Html.PartialAsync("_ProductItem", abc)
Hoặc
    @{
        await Html.RenderPartialAsync("_ProductItem", abc)
    }

- Trong file Partial  cũng phải có thêm chỉ thị để nhận dữ liệu  model là gì
    @model ABC      // Model bây giờ có cấu trúc là class ABC

# ----------------------------------------------------------------
# Component
- Cũng giống <Partial> và <RazorPage> -->> RazorPage mini.
- Trong Component, ta có thể Inject các Service

- Cách chèn một Component vào file cshtml 
    @await Component.InvokeAsync("ProductBox")

- Nếu Component có tham số thì truyền sau tên Component(ProductBox là tên folder  nằm trong thư mục Pages/Shared/Components)
    @await Component.InvokeAsync("ProductBox", ts1, ts2..)

- Thưc mục chứa các Component là thư mục tên <Components> nằm trong thư mục Shared
    Pages/Views
        Shared
            Components
                ProductBox
                    ProductBox.cs
                    ProductBox.cshtml

## ProductBox.cs
- Là một lớp cần thiết để tạo componet. 
    - DK: Trong lớp này cũng cần có phương thức tên Invoke hoặc  InvokeAsync trả về một chuổi(string) hoặc (IViewComponentResult)
        public string Invoke/InvokeAsync(parameter1, ....){}
    - DK: 
        - Phải có thêm thuộc tính attribute [ViewComponent] ngay đầu class
        - Hoặc có thể đặt tên class có hậu tố ViewComponent, ProductBoxViewComponent
        - Hoặc kế thừa từ class ViewComponent, ProductBox : ViewComponent
    
- Nếu trả về một IViewComponentResult thì return View(). View này mặc định là file Default.cshtml
    - nều return View("abc) thì trả về code trong file abc.cshtml cùng thư mục
- Trong file Default.cshtml sẽ viết code như file page

# ----------------------------------------------------------------
# Truyền dữ liệu Model từ file cs ViewComponent qua View
- Cách truyền trong hàm Invoke
    return View<string>("Hello world")  
- Trong file Default.cshtml sẽ có chỉ thị model
    @model string

# ----------------------------------------------------------------
#  Quy trình
- Vào đường dẫn Pages/Shared/Components
- Tạo folder ABC (là folder chứa có tạo Component ABC)

- Tạo file ABC.cs (là file để xử lý Backend, có thể nhận dữ liệu truyền vào từ hàm Invoke, hoặc Inject phương thức khởi tạo các service)
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    public class UserBox : ViewComponent
    {
        private readonly IMockData _mockData;
        public UserBox(IMockData mockData)
        {
            this._mockData = mockData;
            
        }
        public IViewComponentResult Invoke(List<string> ls)
        {
            var data = _mockData.GetData();

            return View(ls);
        }
    }

- Tạo file Default.cshtml để viết code html và có chỉ thị model để sử dụng dữ từ Component gửi lên

- Gọi và truyển dữ liệu
- @{
    // ls là dữ liệu truyền từ page vào Component
    var ls = new List<string>()
    {
    "Huỳnh Ngọc Sơn",
    "Huỳnh Ngọc Sơn2",
    "Huỳnh Ngọc Sơn3"
    };
}

@* Ở đây ta gọi Component UserBox, và truyền dữ liệu ls *@
@await Component.InvokeAsync("UserBox", ls)    

# ------------------------------------------------------------
# Get data from HttpRequest 
- Các cách lấy dữ liệu từ biến Request(HttPRequest).
- VD ta có chỉ thị URL: "/order/{id}". Và ta truy cập vào URL: "/order/1?color=red&bg=green"
    HttpRequest.Query: là giá trị của <color> và <bg>
    HttpRequest.RouteValues: giá trị của <id>
    HttpRequest.Form: các giá trị mà form của trang đó gửi lên

# ----------------------------------------------------------------
# Get data from Parameter của Action
- Cách này ta sẽ  đặt tên các biến trùng tên với các <Tên Query>, <Tên Route>, Tên các biến không phân biệt hoa thường
- VD ta có chỉ thị URL: "/order/{productid}". Và ta truy cập vào URL: "/order/1?color=red&bg=green"
- Ta có một Action OnGet có các tên tương ứng <Tên Query>, <Tên Route>
    public void OnGet(int ProductId, string color, string bg)
    - ProductId = 1
    - color = red
    - bg = green
- Giá trị của int? là null, int là 0, string là null

# ----------------------------------------------------------------
# Binding data from ROUTE, QUERY, FORM (Property)
- Mặc định thì các giá trị sẽ được gán khi có phương thức Post tới(Cũng có thể get).  Khi phương thức Post tới thì nó sẽ ưu tiên gán các biến từ chỉ thị @url sau đó là form vào thuộc tính, sau đó là query
- Giả sử ta đang nói đến thuộc tính Email
- Binding các thuộc tính tên email, Email, EmaIL... từ <form> của phương thức Post
    [BindProperty]
- Binding đến thuộc tính email111
    [BindProperty(Name = "email111")]
- Thiết lập binding ngay cả trên phương thức get
    [BindProperty(SupportsGet = true)]
- Thiết lập cho class  thì các thuộc tính không cần phải khai báo lại nhiều, nhưng không bao quát
    [BindProperties]
- Ngoài ra còn có các các  thuộc tính
    [Bind("Email", "UserName")]     // áp dụng cho class để binding đến 2 thuộc tính
    [BindRequired]                  // Bắt buộc phải binding đến thuộc tính đó, nếu không có là lỗi
    [BindNever]                     // không thực hiện binding

- Ta có thể chỉ định rõ nguồn Binding để nó ưu tiên lấy
    [FromQuery]     - lấy giá trị từ chuỗi query của URL
    [FromRoute]     - lấy giá trị từ Route
    [FromForm]      - lấy giá Form gửi đến
    [FromBody]      - lấy giá từ Body của Http Request
    [FromHeader]    - lấy giá từ Header của Http Request

# ----------------------------------------------------------------
# TagHelper data from
- Nếu ta dùng <asp-for> cho thẻ Label thì nó sẽ hiển thị Name trong thuộc tính Display đã quy định trong class 
- Nếu ta dùng <asp-for> cho thẻ input thì nó sẽ lấy giá trị gán vào
- Nếu ta dùng <asp-validation-for> thì nó sẽ hiện thị ra dòng lỗi nếu như giá trị truyền vào biến không trung khớp với các diều kiện

# ------------------------------------------------------------
# Razor and MVC
- Thêm option vào service để các url chuyển thành chữ thương hết
    services.Configure<RouteOptions>(options=>{
        options.LowercaseUrls= true;
    });
# ------------------------------------------------------------
# Thẻ text      &&      @:
- là thẻ có chức năng show ra text và không nằm trong thẻ nào, dùng để chèn code html vào cs để show lên client mà không cần dùng thẻ
    <text>@abc</text>
hoặc
    @: abc

# ------------------------------------------------------------
# @RenderSection("TenSection", required: false)
- Là chỉ thị render ra một vùng section nhỏ vào vị trí đó, <required:false> tức là  các file page có cũng được không có cũng được

# ----------------------------------------------------------------
# @section TenSection{}
- Là chỉ thị tạo ra code để render vào vị trí đặt chỉ thị @RenderSection trong file _layout.cshtml

# ------------------------------------------------------------
# Partial View
- Giúp Chia nhỏ file page thành nhiều file
- Giúp tài sử dụng ở nhiều trang khác nhau
- Khi tạo Partial thì không có chỉ thị @page trên đầu trang

- Chèn Partial vào page
    <partial  name="_ProductItem" />

- Cách 2 để chèn
    @await Html.PartialAsync("_ProductItem")

- Cách 3: chèn vào vị trí code c#
    @{
        await Html.RenderPartialAsync("_ProductItem")
    }

# ------------------------------------------------------------
# Truyền dữ liệu vào Partial
- Ta cần tạo Model class ABC  cho nó
- Vào truyền đối tượng của model class ABC đó qua TagHelper model (abc là đối tượng của lớp ABC)
    <partial name="_ProductItem" model="abc"/>
Hoặc
    @await Html.PartialAsync("_ProductItem", abc)
Hoặc
    @{
        await Html.RenderPartialAsync("_ProductItem", abc)
    }

- Trong file Partial  cũng phải có thêm chỉ thị để nhận dữ liệu  model là gì
    @model ABC      // Model bây giờ có cấu trúc là class ABC

# ------------------------------------------------------------
# Component
- Cũng giống <Partial> và <RazorPage> -->> RazorPage mini.
- Trong Component, ta có thể Inject các Service

- Cách chèn một Component vào file cshtml 
    @await Component.InvokeAsync("ProductBox")

- Nếu Component có tham số thì truyền sau tên Component(ProductBox là tên folder  nằm trong thư mục Pages/Shared/Components)
    @await Component.InvokeAsync("ProductBox", ts1, ts2..)

- Thưc mục chứa các Component là thư mục tên <Components> nằm trong thư mục Shared
    Pages/Views
        Shared
            Components
                ProductBox
                    ProductBox.cs
                    ProductBox.cshtml

## ProductBox.cs
- Là một lớp cần thiết để tạo componet. 
    - DK: Trong lớp này cũng cần có phương thức tên Invoke hoặc  InvokeAsync trả về một chuổi(string) hoặc (IViewComponentResult)
        public string Invoke/InvokeAsync(parameter1, ....){}
    - DK: 
        - Phải có thêm thuộc tính attribute [ViewComponent] ngay đầu class
        - Hoặc có thể đặt tên class có hậu tố ViewComponent, ProductBoxViewComponent
        - Hoặc kế thừa từ class ViewComponent, ProductBox : ViewComponent
    
- Nếu trả về một IViewComponentResult thì return View(). View này mặc định là file Default.cshtml
    - nều return View("abc) thì trả về code trong file abc.cshtml cùng thư mục
- Trong file Default.cshtml sẽ viết code như file page

# ------------------------------------------------------------
# Truyền dữ liệu Model từ file cs ViewComponent qua View
- Cách truyền trong hàm Invoke
    return View<string>("Hello world")  
- Trong file Default.cshtml sẽ có chỉ thị model
    @model string  

# ------------------------------------------------------------
#  Quy trình tạo Component
- Vào đường dẫn Pages/Shared/Components
- Tạo folder ABC (là folder chứa có tạo Component ABC)

- Tạo file ABC.cs (là file để xử lý Backend, có thể nhận dữ liệu truyền vào từ hàm Invoke, hoặc Inject phương thức khởi tạo các service)
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    public class UserBox : ViewComponent
    {
        private readonly IMockData _mockData;
        public UserBox(IMockData mockData)
        {
            this._mockData = mockData;
            
        }
        public IViewComponentResult Invoke(List<string> ls)
        {
            var data = _mockData.GetData();

            return View(ls);
        }
    }

- Tạo file Default.cshtml để viết code html và có chỉ thị model để sử dụng dữ từ Component gửi lên

- Gọi và truyển dữ liệu
- @{
    // ls là dữ liệu truyền từ page vào Component
    var ls = new List<string>()
    {
    "Huỳnh Ngọc Sơn",
    "Huỳnh Ngọc Sơn2",
    "Huỳnh Ngọc Sơn3"
    };
}

@* Ở đây ta gọi Component UserBox, và truyền dữ liệu ls *@
@await Component.InvokeAsync("UserBox", ls)
 
# ------------------------------------------------------------
# Binding data from ROUTE, QUERY, FORM (Property)
- Mặc định thì các giá trị sẽ được gán khi có phương thức Post tới(Cũng có thể get).  Khi phương thức Post tới thì nó sẽ ưu tiên gán các biến từ chỉ thị @url sau đó là form vào thuộc tính, sau đó là query
- Giả sử ta đang nói đến thuộc tính Email
- Binding các thuộc tính tên email, Email, EmaIL... từ <form> của phương thức Post
    [BindProperty]
- Binding đến thuộc tính email111
    [BindProperty(Name = "email111")]
- Thiết lập binding ngay cả trên phương thức get
    [BindProperty(SupportsGet = true)]
- Thiết lập cho class  thì các thuộc tính không cần phải khai báo lại nhiều, nhưng không bao quát
    [BindProperties]
- Ngoài ra còn có các các  thuộc tính
    [Bind("Email", "UserName")]     // áp dụng cho class để binding đến 2 thuộc tính
    [BindRequired]                  // Bắt buộc phải binding đến thuộc tính đó, nếu không có là lỗi
    [BindNever]                     // không thực hiện binding

- Ta có thể chỉ định rõ nguồn Binding để nó ưu tiên lấy
    [FromQuery]     - lấy giá trị từ chuỗi query của URL
    [FromRoute]     - lấy giá trị từ Route
    [FromForm]      - lấy giá Form gửi đến
    [FromBody]      - lấy giá từ Body của Http Request
    [FromHeader]    - lấy giá từ Header của Http Request

# ------------------------------------------------------------
# TagHelper data from
- Nếu ta dùng <asp-for> cho thẻ Label thì nó sẽ hiển thị Name trong thuộc tính Display đã quy định trong class 
- Nếu ta dùng <asp-for> cho thẻ input thì nó sẽ lấy giá trị gán vào
- Nếu ta dùng <asp-validation-for> thì nó sẽ hiện thị ra dòng lỗi nếu như giá trị truyền vào biến không trung khớp với các diều kiện

# ------------------------------------------------------------
    
# ------------------------------------------------------------
    
# ------------------------------------------------------------
    
# ------------------------------------------------------------
# Attributes
- Required
- Display(Name)
- EmailAddress
- Compare
- DataType


# ------------------------------------------------------------
    
# ------------------------------------------------------------
    
# ------------------------------------------------------------