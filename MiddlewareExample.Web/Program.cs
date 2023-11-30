using MiddlewareExample.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

#region Use ve Run kullan�m�

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Before First Middleware\n");

//    await next();

//    await context.Response.WriteAsync("After First Middleware\n");

//});

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Before Second Middleware\n");

//    await next();

//    await context.Response.WriteAsync("After Second Middleware\n");

//});

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Terminal Third Middleware\n");
//});

#endregion

#region Map kullan�m�

//app.Map("/ornek", app =>
//{
//    app.Run(async context =>
//    {
//        await context.Response.WriteAsync("Ornek URL i�in Middleware.");
//    });
//});

#endregion

#region MapWhen kullan�m�

//app.MapWhen(context => context.Request.Query.ContainsKey("name"), app =>
//{
//    app.Use(async (context,next) =>
//    {
//        await context.Response.WriteAsync("Before First Middleware.\n");

//        await next();

//        await context.Response.WriteAsync("After First Middleware.\n");
//    });

//    app.Run(async context =>
//    {
//        await context.Response.WriteAsync("Terminal Second Middleware.\n");
//    });
//});

#endregion

app.UseMiddleware<WhiteIpAddressControlMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
