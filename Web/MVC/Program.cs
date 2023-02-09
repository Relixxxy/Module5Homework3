using MVC;
using MVC.Services;
using MVC.Services.Interfaces;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<AppSettings>(configuration);

builder.Services.AddHttpClient();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute("default", "{controller=Catalog}/{action=Index}/{id?}");
app.MapControllerRoute("defaultError", "{controller=Error}/{action=Error}");
app.MapControllers();

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}