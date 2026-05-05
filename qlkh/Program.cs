using Microsoft.EntityFrameworkCore; // Thêm dòng này
using qlkh.Data; // Đảm bảo namespace này trỏ đúng đến file ApplicationDbContext của bạn
using qlkh.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- BẮT ĐẦU PHẦN THÊM MỚI ---
// 1. Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Đăng ký ApplicationDbContext vào hệ thống
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // Thay UseSqlServer bằng UseSqlite nếu bạn dùng SQLite
// --- KẾT THÚC PHẦN THÊM MỚI ---

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.EnrollmentStatuses.Any(s => s.StatusCode == "PENDING"))
    {
        db.EnrollmentStatuses.Add(new EnrollmentStatus
        {
            StatusCode = "PENDING",
            StatusName = "Cho xac nhan"
        });
    }

    if (!db.PaymentMethods.Any(m => m.MethodCode == "VIETQR"))
    {
        db.PaymentMethods.Add(new PaymentMethod
        {
            MethodCode = "VIETQR",
            MethodName = "VietQR"
        });
    }

    if (!db.PaymentStatuses.Any(s => s.StatusCode == "PENDING"))
    {
        db.PaymentStatuses.Add(new PaymentStatus
        {
            StatusCode = "PENDING",
            StatusName = "Cho thanh toan"
        });
    }

    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
