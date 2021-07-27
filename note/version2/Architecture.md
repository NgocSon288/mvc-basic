# 1. Tạo project
- Tạo project bằng lệnh terminal hoặc bằng visual studio 2019(.NET Core M-V-C)
    <!-- dotnet new mvc -->

# ------------------------------------------------------------
# 2. HTTPS
- Truy vấn ứng dụng với HTTPs
    <!-- 
        dotnet dev-certs https --clean
        dotnet dev-certs https --trust
     -->

# ------------------------------------------------------------
# 3. Giá trị trả về của Controller  
- Trả về [ContentResult]
    <!-- 
        public IActionResult GetContent () {
            // mime - tham khảo https://en.wikipedia.org/wiki/Media_type
            string contentType = "text/plain";
            string content = "Đây là nội dung file văn bản";
            return Content (content, contentType, Encoding.UTF8);
        } 
    -->

- Trả về [Json]
    <!-- 
        public IActionResult GetJson()
        {
            return Json(
                new {
                    key1 = 100,
                    key2 = new string[] {"a", "b",  "c"}
                }
            );
        }
    -->

- Trả về [File]
    <!-- 
        public IActionResult FileAnh()
        {
            string filepath = "logo.png";
            return File(filepath, "image/png");
        }
    -->

- Chuyển hướng
    <!-- 
        public IActionResult TestRedirect()
        {
            return Redirect("https://xuanthulab.net");                              // Redirect 302 - chuyển hướng sang URL khác
            return RedirectToRoute(new {controller="Home", action="Index"});        // Redirect 302 - Found
            return RedirectPermanent("https://xuanthulab.net");                     // Redirect 301 - chuyển hướng URL khác
            return RedirectToAction("Index");                                       // Chuyển hướng sang Action khác
        } 
    -->

# ------------------------------------------------------------
# 4. Cấu hình RouteOptions
- Cấu hình RouteOptions.
    <!--
        services.Configure<RouteOptions> (options => {
            options.AppendTrailingSlash = false;        // Thêm dấu / vào cuối URL
            options.LowercaseUrls = true;               // url chữ thường
            options.LowercaseQueryStrings = false;      // không bắt query trong url phải in thường
        }); 
    -->

- Đặt tên route cho controller
    <!--  
        [Route("test", Name = "testroute")]
        public IActionResult Index()
        {
            return View();
        }
    -->

- Chuyển hướng đến URL khi biết tên route
    <!-- public IActionResult Test() => RedirectToRoute("testroute"); -->

- Tạo URL từ route name
    <!-- var url = Url.RouteUrl("testroute", new {id = 100});  // url = /test/100  -->

# ------------------------------------------------------------
# 5. Đăng ký Service
- Đăng ký class thành service
    <!-- services.AddSingleton<IBookRepository, BookRepository>(); -->

# ------------------------------------------------------------
# 6. Using LibMan
## Dùng Visual studio
- Chọn project -> Add -> Client-Side Library
- Search thư viện cần dùng và nhấn install
-> Sau đó nó sẽ tự add vào wwwroot/lib.. và sinh ra file [libman.json]
- Có thể nhấn vào [Restore] hoặc [Clean] file [libman.json] để xóa hoặc cài đặt

## Dùng VS Code
- cài đặt libman
    <!-- dotnet tool install -g Microsoft.Web.LibraryManager.Cli -->
- Kiểm tra version
    <!-- libman --version -->
- Tạo file [libman.json]
    <!-- libman init -->
- Muốn cài thư viện nào thì cài
    <!-- libman install twitter-bootstrap -->
- Tải các thư viện có trong file [libman.json]      
    <!-- libman restore -->
- Update một  thư viện
    <!-- libman update jquery -->

# ------------------------------------------------------------
# 7. EF Core
- Cài đặt Package và tool
    dotnet tool install --global dotnet-ef
    dotnet tool install --global dotnet-aspnet-codegenerator
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer

- Tạo lớp DBContext
- Tạo ConnectionStrings trong appsettings.json
    <!-- 
        "ConnectionStrings": {
            "Default": "Data Source=NGOCSON\\SQLEXPRESS01; Initial Catalog=mvc01; Integrated Security=true"
        }, 
    -->
- Đăng ký service AddDbContext
    <!-- 
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(_configuration.GetConnectionString("Default"));
        }); 
    -->

# ------------------------------------------------------------
# 8. Thiết lập IOption
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
# 9. Email
- Cài đặt package
    <!-- 
        dotnet add package MailKit
        dotnet add package MimeKit
     -->

- Tạo cấu hình mail trong appsettings.json
    <!-- 
        "MailSettings": {
            "Mail": "sondeptrai2288@gmail.com",
            "DisplayName": "SƠN ĐẸP TRAI UTI",
            "Password": "XXXXXXXXXXX",
            "Host": "smtp.gmail.com",
            "Port": 587
        }
     -->

- Tạo lớp để đăng ký Option và có thể Inject IOptions<>
    <!-- 
        public class MailSettings
        {
            public string Mail { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
        }
     -->

- Tạo lớp [MailContent] chứa thông tin nội dung gửi
    <!-- 
        public class MailContent
        {
            public string To { get; set; }              // Địa chỉ gửi đến
            public string Subject { get; set; }         // Chủ đề (tiêu đề email)
            public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email
        } 
    -->

- Xây dưng Interface [ISendMailService] để dễ dàng đăng ký Servie và Inject
   <!-- 
        public interface ISendMailService {
            Task SendMail(MailContent mailContent);
            
            Task SendEmailAsync(string email, string subject, string htmlMessage);
        }
    -->

- Tạo lớp [SendEmailService] triển khai từ Interface [ISendMailService]
    <!-- 
        public class SendMailService : ISendMailService
        {
            private readonly MailSettings mailSettings;

            public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
            {
                mailSettings = _mailSettings.Value;
            }

            // Gửi email, theo nội dung trong mailContent
            public async Task SendMail(MailContent mailContent)
            {
                var email = new MimeMessage();
                email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
                email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
                email.To.Add(MailboxAddress.Parse(mailContent.To));
                email.Subject = mailContent.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = mailContent.Body;
                email.Body = builder.ToMessageBody();

                // dùng SmtpClient của MailKit
                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                try
                {
                    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                    await smtp.SendAsync(email);
                }
                catch (Exception ex)
                {
                    // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                    System.IO.Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    await email.WriteToAsync(emailsavefile);
                }

                smtp.Disconnect(true);
            }

            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                await SendMail(new MailContent()
                {
                    To = email,
                    Subject = subject,
                    Body = htmlMessage
                });
            }
        }
     -->

- Đăng ký service [Option] cho [MailSettings], và servie cho [SendMailService]
    <!-- 
        services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));
        services.AddScoped<ISendMailService, SendMailService>(); 
    -->
- Sau đó Inject và dùng như bình thường
    <!-- await _sendMailService.SendEmailAsync("18521694@gm.uit.edu.vn", "Xin chào", "<h1>Hello this is text</h1>"); -->

## Note: khi làm việc với mail
- Tạo file [Template.html] chứa code [html], và chèn các ký tự [{{Key}}] để có thể tạo mail dynamic
- Dùng [List<KeyValuePair<String,String>>], hoặc [Dictionary<String,String>] để chứa các [key/value] để có thể [Replace] text được [Read] từ file [Template.html]
- Sau đó ta dùng [ForEach] để [Replace] các [{{Key}}], với [placeholders] là danh sách [key/value]
    <!-- 
        var text = File.ReadAllText("./Template.html");
        foreach(var item in placeholders)
        {
            text = text.Replace($"{{{item.Key}}}", item.Value);
        }
     -->

# ------------------------------------------------------------
# ------------------------------------------------------------
# ------------------------------------------------------------
#                           Identity
# ------------------------------------------------------------
# 0. Most Notes
- Inject đối tượng [SignInManager] để có thể kiểm tra user Login chưa
    @inject SignInManager<UserApp>

# ------------------------------------------------------------
# 1. Thiết lập sử dụng Identity
- Cài đặt các [Package]
    <!-- 
        dotnet add package System.Data.SqlClient
        dotnet add package Microsoft.EntityFrameworkCore
        dotnet add package Microsoft.EntityFrameworkCore.SqlServer  
        dotnet add package Microsoft.EntityFrameworkCore.Design     
        dotnet add package Microsoft.Extensions.DependencyInjection
        dotnet add package Microsoft.Extensions.Logging.Console

        dotnet add package Microsoft.AspNetCore.Identity
        dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
        dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
        dotnet add package Microsoft.AspNetCore.Identity.UI
        dotnet add package Microsoft.AspNetCore.Authentication
        dotnet add package Microsoft.AspNetCore.Abstractions
        dotnet add package Microsoft.AspNetCore.Cookies
        dotnet add package Microsoft.AspNetCore.Facebook
        dotnet add package Microsoft.AspNetCore.Google
        dotnet add package Microsoft.AspNetCore.jwtBeare
        dotnet add package Microsoft.AspNetCore.MicrosoftAccount
        dotnet add package Microsoft.AspNetCore.oAuth
        dotnet add package Microsoft.AspNetCore.OpenIDConnect
        dotnet add package Microsoft.AspNetCore.Twitter 
    -->

- Thiết lập Identity trong [ConfigureServices], ở đây t đang dùng [IdentityUser] và [IdentityRole] mặc định, ta có thể tạo ra một lớp khác và kế thừa  lại từ lớp đó. Và lớp  [DbContext] là [AppDbContext]
    <!-- 
        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>(); 
    -->

- Thiết lập Middleware trong [Configure]
    <!-- app.UseAuthentication();   // Phục hồi thông tin đăng nhập (xác thực) -->

- Cho lớp [DbContext] kế từ lớp [IdentityDbContext] thay vì là [DbContext]. Có thể dùng các hàm để sửa lại tên của các Table
    <!-- public class AppDbContext : IdentityDbContext -->              // Mặc định thì nó sử dụng IdentityUser là User
    <!-- public class AppDbContext : IdentityDbContext<AppUser> -->     // Đây là ta đang dùng lớp AppUser để làm User, [AppUser] kế thừa từ [IdentityUser]

- Thực hiện [Migrations] và update [Database]

# ------------------------------------------------------------
# 2. Custom IDentityUser
- Tạo class [UserApp] kế thừa từ class [IDentityUser]
    <!-- 
        public class UserApp : IdentityUser
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public DateTime? DateOfBirth { get; set; }
        }
     -->
- Chỉ định ra lớp [User] mình sử dụng là [UserApp] trong lớp [DbContext]
    <!-- public class AppDbContext : IdentityDbContext<UserApp> -->

- Cập nhật lại service
    <!-- 
        services.AddIdentity<UserApp, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
     -->

- Sau đó, thực hiện cập nhật DB bằng migrations

# ------------------------------------------------------------
# 3. Thiết lập các Options, cho UserApp
- Thiết lập các option trong ConfigureService
    <!--
        services.Configure<IdentityOptions> (options => {
            // Thiết lập về Password
            options.Password.RequireDigit = false; // Không bắt phải có số
            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

            // Cấu hình Lockout - khóa user
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
            options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
            options.Lockout.AllowedForNewUsers = true;

            // Cấu hình về User.
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;  // Email là duy nhất

            // Cấu hình đăng nhập.
            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại

        });
    -->
 
# ------------------------------------------------------------
# 4. Sign up (Đăng ký)
- Tạo lớp Model với các Attributes cần thiết
- Controller/Action Signup, Add view, có thể add view [Create] tương ứng với [SignupUserModel] cho nhanh
- [UserManager<IdentityUser>] là đối tượng quản lý [CRID] [User], ta có thể tạo một Repository để thực hiện Inject [UserManager].
- Tạo phương thức [Create] bằng [UserManager] cơ bản
    <!-- 
        public async Task<IdentityResult> Create(SignUpUserModel model)
        {
            var user = new IdentityUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            return await _userManager.CreateAsync(user, model.Password);
        } 
    -->
- Đăng ký [Service]
- Inject vào [Controller] sử dụng

- Khi dùng
    - Đối tượng [IdentityResult] ta có thể kiểm tra [Success] danh sách các [Error] nếu tạo không thành công
    <!-- 
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }  
    -->


# ------------------------------------------------------------
# 5. Sign in(Đăng nhập)
- Tạo lớp Model với các Attributes cần thiết
- Tạo action login, add view có thể add view [Create] tương ứng với [SigninModel] cho nhanh
- [SignInManager<UserApp>] là đối tượng quản lý login, logout [User], ta có thể tạo một Repository để thực hiện Inject [SignInManager].
- Tạo phương thức Signin, và signin bằng phương thức [_signInManager.PasswordSignInAsync]
    <!-- 
        public async Task<SignInResult> Login(SigninModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        } 
    -->   
- [SignInResult] là một đối tượng chứa các thuộc tính
    - [Succeeded]: đăng nhập có thành công không
    - [IsLockedOut]: tài khoản có bị lock không, tùy thuộc vào mình có cấu hình ở Configure hay không, nằm ở [options.Lockout]
    - [IsNotAllowed]: có cho phép tài khoản đăng nhập mà [ConfirmEmail] hay không, tùy thuộc vào cấu hình ở [options.SignIn]
    - [RequiresTwoFactor]: xác thực 2 bước khi đăng nhập

# ------------------------------------------------------------
# 6. Signout(Đăng xuất)
- Dùng phương thức [SignOutAsync] của [SignInManager] để đăng xuất


# ------------------------------------------------------------
# 7. Change Password(Đổi mật khẩu)
- Tạo lớp Model với các Attributes cần thiết
- Tạo action changepassword, add view có thể add view [Create] tương ứng với [ChangePassword] cho nhanh
- Ta có thể dùng phương thức [*] để đổi mật khẩu, tuy nhiên nó cần truyền vào một [User] chính là [UserApp] hiện tại. vì vậy ta cần lấy ra [ID] của user hiện tại, sau đó tìm ra [User] theo [ID] đó. Ta có thể Inject [IUserService] của bước [11] vào.
    <!-- public virtual Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword); -->

- Phương thức đó là 
    <!-- 
        public async Task<IdentityResult> ChangePassword(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }
     -->

# ------------------------------------------------------------
# 8. xxxxxxxxxxxxx

# ------------------------------------------------------------
# 9. Authorize
- Vào Connfigure sử dụng Authorization
    <!-- app.UseAuthorization(); -->
- Lúc này nều ta có thêm [Attribute] [Authorize] thì nó sẽ chuyển hướng đến trang đăng nhập mà ta [Config] nếu chưa đăng nhập
- Cấu hình lại đường dẫn đăng nhập
    <!-- 
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/login";
        }); 
    -->

# ------------------------------------------------------------
# 10. Claims in ASP.NET core identity
- Tạo lớp Helper
    <!--
        public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<UserApp, IdentityRole>
        {
            public ApplicationUserClaimsPrincipalFactory(UserManager<UserApp> userManager,
                RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
                : base(userManager, roleManager, options)
            {
            }

            protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserApp user)
            {
                var identity = await base.GenerateClaimsAsync(user);
                identity.AddClaim(new Claim("UserFirstName", user.FirstName ?? ""));
                identity.AddClaim(new Claim("UserLastName", user.LastName ?? ""));
                return identity;
            }
        }
    -->

- Đăng ký dịch vụ với ConfigureServices
    <!-- services.AddScoped<IUserClaimsPrincipalFactory<UserApp>, ApplicationUserClaimsPrincipalFactory>(); -->

- Có thể dùng với [Claim] đã add vào trong phương thức [GrenerateClaimAsync]
    <!-- @User.FindFirst("UserFirstName").Value -->

# ------------------------------------------------------------
# 11. User service
- Tạo lớp [UserService] để thực hiện các chức năng cần thiết
    <!--
        public class UserService : IUserService
        {
            private readonly IHttpContextAccessor _httpContext;

            public UserService(IHttpContextAccessor httpContext)
            {
                _httpContext = httpContext;
            }
            public string GetUserId()
            {
                return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            public bool IsAuthenticated()
            {
                return _httpContext.HttpContext.User.Identity.IsAuthenticated;
            }
        }
    -->

- Đăng ký [service] và dùng


# ------------------------------------------------------------
# 12. Using token để confirm email khi tạo tài khoản
- Muốn dùng thì thêm [AddDefaultTokenProviders] vào [ConfigureService]
    <!-- 
        services.AddIdentity<UserApp, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    -->

- Chèn vào giữa [IdentityResult], nếu tạo được tài khoản thì tạo ra [code-token] -> chuẩn hóa thành [coke-url] -> tạo [callback-url] -> gửi mail, trong phần này cần
    - [domain] cần lưu ở appsettings.json
    - [callbackUrl]: cần tạo bằng phương thức có sẵn, [callbackUrl] có tác dụng gọi lại Action [Verify] mình tạo trước, trong [Action] đó nhận các tham số [userId] để [vertify] cho user đó, [code] để kiểm tra có hợp lệ không
    - [html] cần tạo file template.cshtml để có thể chèn nhiều nội dung hơn
    <!--  
        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var domain = "https://localhost:44305";
            var callbackUrl = $"{domain}/verify-signup?userId={user.Id}&code={code}";
            var html = $"<h1>Click <a href='{callbackUrl}'>Here</a> to verify</h1>";

            await _sendMailService.SendEmailAsync(user.Email, "Verify Email to Singup", html);
        }
    -->

- Confirm bằng phương thức [ConfirmEmailAsync] của [userManager]
    - [code] ta nhận được là [string] đã được mã hóa, ta cần giải mã nó
    <!--  
        public async Task<IdentityResult> ConfirmSignup(string userId, string code)
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.ConfirmEmailAsync(user, code);
        }
    -->
    

# ------------------------------------------------------------
# 13. Roles
- Thuộc tính [Authorize] là đăng nhập mới được vào
- [Authorize(Roles="Admin")] là phải có quyền [Admin] mới vào được

# ------------------------------------------------------------
# 14. Lifespan (Thiết lập thời gian cho Token)
- Thiết lập  thời gian tồn tại cho Token
    <!-- 
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(5);
        }); 
    -->