using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login/default";
        options.LogoutPath = "/Account/Logout";
    });

// 以固定連線字串的方式來連線資料庫
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 以動態的方式來連線資料庫
// Register the DbContextFactory
builder.Services.AddSingleton<ClinicApplication.Services.DbContextFactory>();

// Register any services that depend on dynamic DbContexts
builder.Services.AddScoped<ClinicApplication.Services.ClinicService>();

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "defaultLogin",
    pattern: "account/login/default",
    defaults: new { controller = "Account", action = "DefaultLoginPage" });

app.MapControllerRoute(
    name: "sunnyLogin",
    pattern: "account/login/sunny",
    defaults: new { controller = "Account", action = "SunnyLoginPage" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseAuthentication();
app.UseAuthorization();

app.Run();