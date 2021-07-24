# Dùng EF core
- Cài các package
    dotnet tool install --global dotnet-ef
    dotnet tool install --global dotnet-aspnet-codegenerator
    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer

- Tạo các models tương ứng trong DB, tạo lớp DBContext
    <!-- 
        public ArticleContext(DbContextOptions<ArticleContext> options) : base(options)
        {
            Phương thức khởi tạo này chứa options để kết nối đến MS SQL Server
            Thực hiện điều này khi Inject trong dịch vụ hệ thống
        } 
    -->

- Tạo ConnectionStrings trong appsettings.json. Khóa phải là <ConnectionStrings>
    <!-- 
    "ConnectionStrings": {
        "ArticleContext":"Data Source=localhost,1433; Initial Catalog=ArticalDB; UID=SA;PWD=admin1234"
    }, 
    -->

- Đăng ký  service trong ConfigureServices
    <!-- 
        services.AddDbContext<ArticleContext>(options =>
        {
            string connectstring = Configuration.GetConnectionString("ArticleContext");
            options.UseSqlServer(connectstring);
        }); 
    -->

- Dùng các lệnh ef để cập nhật class tạo bảng vào DB(docker)
    dotnet ef migrations add Initial_DB
    dotnet ef database update

- Tạo lớp Service để tạo các service thao tác DB cho tiện.  Ta có thể Inject đối tượng context vào
    <!-- 
        public class ArticleService : IArticleService
        {
            private readonly ArticleContext _context;
            public ArticleService(ArticleContext context)
            {
                _context = context;
            }
            public List<Article> GetAll()
            {
                return _context.Article.ToList();
            }
        } 
     -->

- Đăng ký service đó ở ConfigureServices