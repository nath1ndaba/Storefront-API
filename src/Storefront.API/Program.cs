using Serilog;
using Storefront.API.Extensions;
using Storefront.API.Middleware;
using Storefront.Infrastructure.Data;
using Storefront.Domain.Entities;
using Storefront.Domain.Enums;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Application", "StorefrontAPI")
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/storefront-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}",
        retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    Log.Information("Starting Storefront API");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();


    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddOpenApiDocument(config =>
    {
        config.Title = "Storefront API";
        config.Version = "v1";
        config.Description = "A clean e-commerce API with CQRS architecture";
    });

    // Application & Infrastructure services
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    var app = builder.Build();

    // Seed data
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Products.Any())
        {
            Log.Information("Seeding database with sample products");

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with 6 buttons",
                    Price = 29.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Wireless+Mouse",
                    Category = ProductCategory.Electronics,
                    StockQuantity = 150,
                    Sku = "ELEC-MOUSE-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Gaming Keyboard",
                    Description = "RGB mechanical gaming keyboard",
                    Price = 89.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Gaming+Keyboard",
                    Category = ProductCategory.Electronics,
                    StockQuantity = 75,
                    Sku = "ELEC-KB-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Cotton T-Shirt",
                    Description = "Comfortable 100% cotton t-shirt",
                    Price = 19.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=T-Shirt",
                    Category = ProductCategory.Clothing,
                    StockQuantity = 200,
                    Sku = "CLOTH-SHIRT-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Running Shoes",
                    Description = "Lightweight running shoes for all terrains",
                    Price = 79.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Running+Shoes",
                    Category = ProductCategory.Sports,
                    StockQuantity = 100,
                    Sku = "SPORT-SHOE-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Coffee Maker",
                    Description = "12-cup programmable coffee maker",
                    Price = 49.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Coffee+Maker",
                    Category = ProductCategory.Home,
                    StockQuantity = 50,
                    Sku = "HOME-COFFEE-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Bestseller Novel",
                    Description = "Top-rated fiction bestseller",
                    Price = 14.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Novel",
                    Category = ProductCategory.Books,
                    StockQuantity = 300,
                    Sku = "BOOK-NOVEL-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Yoga Mat",
                    Description = "Non-slip yoga mat with carrying strap",
                    Price = 24.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Yoga+Mat",
                    Category = ProductCategory.Sports,
                    StockQuantity = 120,
                    Sku = "SPORT-YOGA-001",
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Face Cream",
                    Description = "Moisturizing face cream with SPF 30",
                    Price = 34.99m,
                    ImageUrl = "https://via.placeholder.com/300x300?text=Face+Cream",
                    Category = ProductCategory.Beauty,
                    StockQuantity = 80,
                    Sku = "BEAUTY-CREAM-001",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            Log.Information("Database seeded with {Count} products", products.Count);
        }
    }

    // Middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseCors("AllowAll");

    // ONLY NSwag
    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();      // /swagger/v1/swagger.json
        app.UseSwaggerUi();    // /swagger
    }

    // Map endpoints
    app.MapApiEndpoints();

    app.MapGet("/", () => Results.Ok(new
    {
        message = "Storefront API",
        timestamp = DateTime.UtcNow,
        swagger = "/swagger"
    }))
    .WithName("Root")
    .ExcludeFromDescription();

    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.Information("Application closed gracefully");
    Log.CloseAndFlush();

}
