using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaManager.Models;
using SocialMediaManager.Data;
using SocialMediaManager.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SocialMediaManager.BgService.Scheduler;

var builder = WebApplication.CreateBuilder(args);


// Add database

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add Identity with User model
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/auth/login");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Facebook API Service
builder.Services.AddHttpClient<FacebookService>();
builder.Services.AddScoped<FacebookService>();

builder.Services.AddHostedService<Scheduler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
