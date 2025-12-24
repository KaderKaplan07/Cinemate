using Cinemate.Data;
using Cinemate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// ✅ HttpClient (HATANIN ASIL ÇÖZÜMÜ)
builder.Services.AddHttpClient();

// Database
builder.Services.AddDbContext<CinemateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
// Mevcut AddIdentity kodunu bununla değiştir:
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Şifre kısıtlamalarını kaldırıyoruz
    options.Password.RequireDigit = false;           // Rakam zorunlu değil
    options.Password.RequireLowercase = false;       // Küçük harf zorunlu değil
    options.Password.RequireUppercase = false;       // Büyük harf zorunlu değil
    options.Password.RequireNonAlphanumeric = false; // Özel karakter (!, @ vb.) zorunlu değil
    options.Password.RequiredLength = 3;             // En az 3 karakter yeterli
    options.User.RequireUniqueEmail = true; // Aynı mail ile 2. kez kayıt olunamaz
})
.AddEntityFrameworkStores<CinemateDbContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// kimlik doğrulama
app.UseAuthentication();

// yetkilendirme
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
