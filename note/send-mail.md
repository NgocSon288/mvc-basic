#  https://xuanthulab.net/asp-net-core-gui-mail-trong-ung-dung-web-asp-net.html
# Thư mục Web07
# ----------------------------------------------------------------
# ----------------------------------------------------------------
# ----------------------------------------------------------------

# Send mail bằng smtp của google
- Nằm trong file MailUtils/MailUtils.cs

# ----------------------------------------------------------------
# Dùng MailKit gửi Mail trong ASP.NET với Gmail
- Cài đặt package
    dotnet add package MailKit
    dotnet add package MimeKit

- Có thể dùng Options để Inject
- Tạo cấu hình mail trong appsettings.json
    "MailSettings": {
        "Mail": "sondeptrai2288@gmail.com",
        "DisplayName": "SƠN ĐẸP TRAI UTI",
        "Password": "XXXXXXXXXXX",
        "Host": "smtp.gmail.com",
        "Port": 587
    }

- Tạo lớp để đăng ký Option và có thể Inject IOptions<>
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

- Thực hiện cấu hình, đăng ký Option Service trong file Startup (Xem lại dòng 234 <Tạo và dùng Options được khởi tạo giá trị từ một file (appsettings.json)>)

- Tạo lớp MailContent để chứa thông tin người nhận
    public class MailContent
    {
        public string To { get; set; }              // Địa chỉ gửi đến
        public string Subject { get; set; }         // Chủ đề (tiêu đề email)
        public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email
    }

- Xây dưng Interface ISendMailService để dễ dàng đăng ký Servie và Inject
    public interface ISendMailService {
        Task SendMail(MailContent mailContent);
        
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

- Tạo lớp SendEmailService triển khai từ Interface ISendMailService
    public class SendMailService : ISendMailService {
        private readonly MailSettings mailSettings;

        private readonly ILogger<SendMailService> logger;


        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public SendMailService (IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger) {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }

        // Gửi email, theo nội dung trong mailContent
        public async Task SendMail (MailContent mailContent) {
            var email = new MimeMessage ();
            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.To.Add (MailboxAddress.Parse (mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody ();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try {
                smtp.Connect (mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate (mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex) {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect (true);

            logger.LogInformation("send mail to " + mailContent.To);

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
        await SendMail(new MailContent() {
                To = email,
                Subject = subject,
                Body = htmlMessage
        });
        }
    }

- Đăng ký dịch vụ gửi mail trong ConfigureService

- Dùng như các địch vụ khác
    // Lấy dịch vụ sendmailservice
    var sendmailservice = context.RequestServices.GetService<ISendMailService>();

    MailContent content = new MailContent {
        To = "xuanthulab.net@gmail.com",
        Subject = "Kiểm tra thử",
        Body = "<p><strong>Xin chào xuanthulab.net</strong></p>"
    };

    await sendmailservice.SendMail(content);

# ----------------------------------------------------------------
# Note
- Nên dùng Dictionary<string, string> để tạo ra Content mail dynamic
# ----------------------------------------------------------------
#
# ----------------------------------------------------------------
#