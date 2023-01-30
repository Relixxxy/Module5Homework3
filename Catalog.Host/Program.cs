using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.Configure<CatalogConfig>(configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.MapDefaultControllerRoute();
app.MapControllers();

CreateDbIfNotExists(app);
app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            DbInitializer.Initialize(context).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}