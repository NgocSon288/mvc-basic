# ----------------------------------------------------------------
# Upload one file
- Các thuộc tính có thể dùng, Kiểu dữ liệu dùng để  upload file là IFromFile,
    [DataType(DataType.Upload)]
    [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
    [Display(Name = "Chọn file upload")]
    [BindProperty]
    public IFormFile FileUpload { get; set; }

- Bên View sẽ thêm các thuôc tính
    enctype="multipart/form-data"           // thuộc tính giúp gửi file
    <input asp-for="@Model.FileUpload" />   // một thẻ input sẽ binding đến thuộc tính FileUpload cúa model, ta đã khai báo trong Model

- Code uploadfile
    if (FileUpload != null)
    {
        var file = Path.Combine(_environment.ContentRootPath, "uploads", FileUpload.FileName);
        // var file = Path.Combine("uploads", FileUpload.FileName); // có thể thay thế bằng
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
            await FileUpload.CopyToAsync(fileStream);
        }
    }

# ----------------------------------------------------------------
# Upload multip files
- Các thuộc tính
    [DataType (DataType.Upload)]
    [FileExtensions (Extensions = "png,jpg,jpeg,gif")]
    [Display (Name = "Chọn file upload")]
    [BindProperty]
    public IFormFile[] FileUploads { get; set; }

- Bên view tương tự
    <input asp-for="@Model.FileUploads" />

- Code upload files, tương tự upload one file chỉ là dùng vòng lập
    if (FileUploads != null) {
         foreach (var FileUpload in FileUploads)
         {
            var file = Path.Combine (_environment.ContentRootPath, "uploads", FileUpload.FileName);
            using (var fileStream = new FileStream (file, FileMode.Create)) {
                await FileUpload.CopyToAsync (fileStream);
            }
        }
    }