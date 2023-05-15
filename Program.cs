using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Food_Scape.Models;
using Food_Scape.Data.Services;
using Food_Scape.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
builder.Configuration.GetConnectionString("DefaultConnection")
?? throw new InvalidOperationException("'DefaultConnection' not found.");

// SendGrid Variables
var sendGridKey = builder.Configuration["SendGrid:ApiKey"];

// ReCaptcha Variables
var reCaptchaSiteKey = builder.Configuration["Recaptcha:SiteKey"];
var reCaptchaSecretKey = builder.Configuration["Recaptcha:SecretKey"];

// Google Maps API
var apiKey = builder.Configuration["GoogleMapsApiKey"];

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<FoodScapeContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FoodScapeContext>();


builder.Services.AddControllersWithViews();

// Send Grid
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.IdleTimeout = TimeSpan.FromDays(1);
});

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
