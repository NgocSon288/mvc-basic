# Thiết lập Identity
- Cài đặt các package
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

- Mặc định thì ASP.NET có cung cấp một Model chứa thông tin user đó là lớp <IdentityUser> kế thừa từ lớp Generic Microsoft.AspNetCore.Identity.IdentityUser<TKey> với kiểu xác định string. Nhưng không dùng trực tiếp, phải kế thừa
    <!-- 
        public class IdentityUser
        {
            public IdentityUser(string userName);
            public virtual bool TwoFactorEnabled { get; set; }
            public virtual bool PhoneNumberConfirmed { get; set; }
            public virtual string PhoneNumber { get; set; }
            public virtual string SecurityStamp { get; set; }
            public virtual bool EmailConfirmed { get; set; }
            public virtual string Email { get; set; }
            public virtual string UserName { get; set; }
            public virtual string Id { get; set; }
            public virtual bool LockoutEnabled { get; set; }
            public virtual int AccessFailedCount { get; set; }
        }   
    -->

## ----------------------------------------------------------------
## AppUser : IdentityUser
- Tạo lớp AppUser kế thừa từ lớp IdentityUser  
- Ngoài lớp IdentityUser, có những lớp sau cũng được sử dụng: IdentityRole,IdentityUserRole , IdentityUserClaim , IdentityUserLogin , IdentityUserToken , IdentityRoleClaim , những lớp này sẽ có trong DbContext của ứng dụng. 

## ----------------------------------------------------------------
## Tạo lớp DbContext  kế thừa từ IdentityContext
- DbContext là lớp biểu diễn CSDL. IdentityContext cũng giống như DbContext nhưng nó đã định nghĩa sẵn các DbSet
    - UserRoles
    - Roles
    - RoleClaims
    - Users
    - UserClaims
    - UserLogins
    - UserTokens

- Đăng ký lớp đó với  services của hệ thông trong ConfigureService  
- Đăng ký các dịch vụ của Identity vào hệ thống
    services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

- Thêm Identity vào pipeline, <Giúp cung cập chức năng lưu thông tin khi người dùng truy cập nằm trong 'HttpContext.User'>
    app.UseAuthentication();   // Phục hồi thông tin đăng nhập (xác thực)
    app.UseAuthorization ();   // Phục hồi thông tinn về quyền của User


## ----------------------------------------------------------------
## Cấu hình cho Identity
-  Truy cập IdentityOptions
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

- Cập nhật migrations, database

# ----------------------------------------------------------------
# Triển khai và đăng ký dịch vụ IEmailSender
- Add các package
    dotnet add package MailKit
    dotnet add package MimeKit

- Setting 1 section trong appsettings.json
    <!-- 
        "MailSettings": {
            "Mail": "sondeptrai2288@gmail.com",
            "DisplayName": "Huỳnh Ngọc Sơn - UIT",
            "Password": "SOn01698182219",
            "Host": "smtp.gmail.com",
            "Port": 587
        } 
    -->

- Xây dựng lớp <MailSettings> để đăng ký services vào IOption, <SendMailService> để thực hiện  chức năng gửi mail  
    <!--
        using System;
        using System.Threading.Tasks;
        using MailKit.Security;
        using Microsoft.AspNetCore.Identity.UI.Services;
        using Microsoft.Extensions.Logging;
        using Microsoft.Extensions.Options;
        using MimeKit;

        namespace Album.Mail {

            // Cấu hình dịch vụ gửi mail, giá trị Inject từ appsettings.json
            public class MailSettings {
                public string Mail { get; set; }
                public string DisplayName { get; set; }
                public string Password { get; set; }
                public string Host { get; set; }
                public int Port { get; set; }

            }

            // Dịch vụ gửi mail
            public class SendMailService : IEmailSender {
                private readonly MailSettings mailSettings;

                private readonly ILogger<SendMailService> logger;

                // mailSetting được Inject qua dịch vụ hệ thống
                // Có inject Logger để xuất log
                public SendMailService (IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger) {
                    mailSettings = _mailSettings.Value;
                    logger = _logger;
                    logger.LogInformation ("Create SendMailService");
                }

                public async Task SendEmailAsync (string email, string subject, string htmlMessage) {
                    var message = new MimeMessage ();
                    message.Sender = new MailboxAddress (mailSettings.DisplayName, mailSettings.Mail);
                    message.From.Add (new MailboxAddress (mailSettings.DisplayName, mailSettings.Mail));
                    message.To.Add (MailboxAddress.Parse (email));
                    message.Subject = subject;

                    var builder = new BodyBuilder ();
                    builder.HtmlBody = htmlMessage;
                    message.Body = builder.ToMessageBody ();

                    // dùng SmtpClient của MailKit
                    using var smtp = new MailKit.Net.Smtp.SmtpClient ();

                    try {
                        smtp.Connect (mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                        smtp.Authenticate (mailSettings.Mail, mailSettings.Password);
                        await smtp.SendAsync (message);
                    } catch (Exception ex) {
                        // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                        System.IO.Directory.CreateDirectory ("mailssave");
                        var emailsavefile = string.Format (@"mailssave/{0}.eml", Guid.NewGuid ());
                        await message.WriteToAsync (emailsavefile);

                        logger.LogInformation ("Lỗi gửi mail, lưu tại - " + emailsavefile);
                        logger.LogError (ex.Message);
                    }

                    smtp.Disconnect (true);

                    logger.LogInformation ("send mail to: " + email);

                }
            }
        }
    -->

- Tiến hành đăng ký Option <MailSettings> như service và đăng ký Service <SendMailService>
    <!-- 
        services.AddOptions ();                                        // Kích hoạt Options
        var mailsettings = Configuration.GetSection ("MailSettings");  // đọc config
        services.Configure<MailSettings> (mailsettings);               // đăng ký để Inject

        services.AddTransient<IEmailSender, SendMailService>();        // Đăng ký dịch vụ Mail
     -->
- Từ giờ chỗ nào cần sử dụng <IEmailSender> để gửi mail thì đã có sẵn trong hệ thống.

# ----------------------------------------------------------------
# Phát sinh code (scaffold) Identity
- Sẽ phát sinh các code (các Model, trang Razor Page, Controller, View) sẵn cho chúng ta
    <!-- 
        dotnet aspnet-codegenerator identity -dc Web14.Data.AppDbContext
    -->
    - <Web14.Data.AppDbContext> là lớp Context kế thừa từ <IdentityDbContext> mà ta dã tạo trên
- Có 2 Inject quan trọng
    <!-- 
        @using Microsoft.AspNetCore.Identity
        @using Album.Models

        @inject SignInManager<AppUser> SignInManager
        @inject UserManager<AppUser> UserManager
     -->


- Các phương thức của 2 Inject
    SignInManager.IsSignedIn(User)          // Kiểm tra có đăng nhập không
    UserManager.GetUserName(User)           // Lấy tên Username hiện tại

# ----------------------------------------------------------------
# Xây dựng trang thông báo - chuyển hướng với ViewComponent
- Controller, PageModel, View, Razor Page đều có thuộc tính User kiểu ClaimsPrincipal là mẩu tin về User của phiên làm việc
- Vào thư mục Pages/Shared/Components/MessagePage 
- Tạo file MessagePage.cs để viết code BE
    <!-- 
        using System.Threading.Tasks;
        using Microsoft.AspNetCore.Mvc;

        namespace XTLASPNET
        {
            [ViewComponent]
            public class MessagePage : ViewComponent
            {
                public const string COMPONENTNAME = "MessagePage";
                // Dữ liệu nội dung trang thông báo
                public class Message {
                    public string title {set; get;} = "Thông báo";     // Tiêu đề của Box hiện thị
                    public string htmlcontent {set; get;} = "";         // Nội dung HTML hiện thị
                    public string urlredirect {set; get;} = "/";        // Url chuyển hướng đến
                    public int secondwait {set; get;} = 3;              // Sau secondwait giây thì chuyển
                }
                public MessagePage() {}
                public IViewComponentResult Invoke(Message message) {
                    // Thiết lập Header của HTTP Respone - chuyển hướng về trang đích
                    this.HttpContext.Response.Headers.Add("REFRESH",$"{message.secondwait};URL={message.urlredirect}");
                    return  View(message);
                }
            }
        } 
    -->
- Tạo file Default.cshtml để viết code view
    <!-- 
        @model Web14.Pages.Shared.Components.MessagePage.MessagePage.Message
        @{
            Layout = "_Layout";
            ViewData["Title"] = @Model.title;
        }
        <div class="card m-4">
            <div class="card-header bg-danger text-light">
                <h4>@Model.title</h3>
            </div>
            <div class="card-body">
                @Html.Raw(Model.htmlcontent)
            </div>
            <div class="card-footer">
                Chuyển đến - <a href="@Model.urlredirect">@Model.urlredirect</a>
            </div>
        </div> 
    -->
- Khi nào muốn dùng thì gọi lại component đó với các setting là đối tượng lớp Message.
    <!-- 
        public IActionResult OnGet()
        {
            return ViewComponent("MessagePage", new Web14.Pages.Shared.Components.MessagePage.MessagePage.Message
            {
                title = "Thông báo quan trọng",
                htmlcontent = "Đây là <strong>Nội dung HTML</strong>",
                secondwait = 5,
                urlredirect = "/PRIVACY"
            });
        } 
    -->

# ----------------------------------------------------------------
#   

## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
# ----------------------------------------------------------------
# SignInManager, UserManager
# Register
- Các đối tượng
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

- Xác thực từ dịch vụ ngoài (Googe, Facebook ... bài này chứa thiết lập)
    public IList<AuthenticationScheme> ExternalLogins { get; set; }
- 
    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

- <var user = new AppUser { UserName = Input.UserName, Email = Input.Email }>: tạo đối tượng AppUser, AppUser kế thừa từ IdentityUser, có đầy đủ thuộc tính của User
- <await _userManager.CreateAsync(user, Input.Password)> dùng phương thức này để tạo tài khoản

- <UserManager.GenerateEmailConfirmationTokenAsync> để phát sinh một mã Token duy nhất theo thông tin của User, mã này được dùng để xác nhận email đăng ký là có thật.

- Tạo ra một URL callback <callbackUrl = /Account/ConfirmEmail?userId=useridxx&code=codexxxx>
    var callbackUrl = Url.Page(
        "/Account/ConfirmEmail",
        pageHandler: null,
        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl},
        protocol: Request.Scheme);

- Gửi mail từ IEmailSender mà ta đã đăng ký Service trước
    <!-- 
    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận địa chỉ email", $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>."); 
    -->
- Nếu ta có cấu hình options cần xác thực email trước khi đăng nhập thì có thể kiểm tra
    <!-- _userManager.Options.SignIn.RequireConfirmedEmail -->

- Chuyển sang trang nào đó, ts 1 là trang, ts 2 là các parameter của Handler Get  của PageModel đó
    <!-- 
        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl }); 
    -->

- <UserManager.SignInAsync(user, isPersistent: false)> Có thể đăng nhập bằng phương thức

- <var user = await _userManager.FindByEmailAsync(email);> tìm user theo email

- <var user = await _userManager.FindByIdAsync (userId);> tìm user theo id

- <await _userManager.ConfirmEmailAsync (user, code);> xác thực email cho user đó

- Hash data
    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    code = Encoding.UTF8.GetString (WebEncoders.Base64UrlDecode (code));

# ----------------------------------------------------------------
# Log out
- <await SignInManager.SignOutAsync()> để dăng xuất
- <signInManager.IsSignedIn(User)> kiểm tra đã đăng nhập chưa

# ----------------------------------------------------------------
# Login
- <await HttpContext.SignOutAsync (IdentityConstants.ExternalScheme);> clear external cookies

- Thử đăng nhập bằng Username hoặc email
    <!-- 
        var result = await _signInManager.PasswordSignInAsync (
            Input.UserNameOrEmail,
            Input.Password,
            Input.RememberMe,
            true
        ); 
    -->

- Thử đăng nhập bằng User tìm được
    <!-- 
        var  result = await _signInManager.PasswordSignInAsync (
            user,
            Input.Password,
            Input.RememberMe,
            true
        ); 
    -->

- <result.RequiresTwoFactor> kiểm tra có phải  xác thức 2 yếu tố

- <result.IsLockedOut> kiểm tra tài khoản có bị khóa không

# ----------------------------------------------------------------
# Lockout
- Tránh tấn công Brute Force (thử password nhiều lần)
- ĐK sử dụng phương thức PasswordSignInAsync của SignInManager, phương thức này có dạng:
    <!-- 
        var result = await _signInManager.PasswordSignInAsync (
            user.UserName,
            Input.Password,
            Input.RememberMe,           // Có lưu cookie - khi đóng trình duyệt vẫn nhớ
            true                        // CÓ ÁP DỤNG LOCKOUT
        );
    -->
    - Nếu lockoutOnFailure = true thì nó sẽ cập nhật vào trường <AccessFailedCount> của CSDL
-  ĐK: cấu hình trong ConfigureService
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (2);  // Khóa 2 phút
    options.Lockout.MaxFailedAccessAttempts = 3;                        // Thất bại 3 lần thì khóa

- <result.IsLockedOut>  kiểm tra tài khoản có bị khóa   

# ----------------------------------------------------------------
# Reset Password
- <userManager.GeneratePasswordResetTokenAsync(user);> Genera code để Reset password
- Quy trình là
    <!-- 
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ResetPassword",
            pageHandler: null,
            values: new { area = "Identity", code },
            protocol: Request.Scheme);

        // Gửi email
        await _emailSender.SendEmailAsync(
            Input.Email,
            "Đặt lại mật khẩu",
            $"Để đặt lại mật khẩu hãy <a href='{callbackUrl}'>bấm vào đây</a>."); 
    -->

- <await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);> để reset password lại, code là code được sinh ra khi ta bấm vào email
# ----------------------------------------------------------------

# ----------------------------------------------------------------
# Views
- <asp-for="Input.ConfirmPassword"> == <asp-for="@Model.Input.ConfirmPassword">

## ----------------------------------------------------------------

## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------
## ----------------------------------------------------------------