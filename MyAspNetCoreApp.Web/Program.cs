using Microsoft.EntityFrameworkCore;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext Configurations
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionStrings"));
});

//DI Container
builder.Services.AddSingleton<IHelper,Helper>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// blog/abc ==> blog controller > article action method çalýþsýn
// blog/ddd ==> blog controller > article action method çalýþsýn
//app.MapControllerRoute(
//    name: "article",
//    pattern: "blog/{*article}",
//    defaults: new {controller = "Blog", action = "Article"});

//app.MapControllerRoute(
//    name: "article",
//    pattern: "{controller=Blog}/{action=Article}/{name}/{id}");

//app.MapControllerRoute(
//    name: "productpages",
//    pattern: "{controller}/{action}/{page}/{pageSize}");

//app.MapControllerRoute(
//    name: "productgetbyid",
//    pattern: "{controller}/{action}/{productId}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllers();

app.Run();
