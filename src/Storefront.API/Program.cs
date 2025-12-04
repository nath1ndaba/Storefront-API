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

    // OpenAPI
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

    // CORS Configuration - Production Ready
    // For this assessment, we allow any origin in production to demonstrate
    // API flexibility. In a real production environment, you would:
    // 1. Specify exact frontend domain(s) using environment variables
    // 2. Implement API keys or authentication for additional security
    // 3. Use a secrets manager for sensitive configuration
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            if (builder.Environment.IsDevelopment())
            {
                // Development: Allow localhost for local testing
                policy.WithOrigins(
                    "http://localhost:5173",
                    "http://localhost:5174",
                    "http://localhost:3000",
                    "https://localhost:5173"
                )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            }
            else
            {
                // Production: Allow any origin (public API for demonstration)
                // In production, replace with: .WithOrigins("https://yourdomain.com")
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            }
        });
    });

    var app = builder.Build();

    // Seed database with sample data
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

    // Middleware Pipeline - Order matters!
    // 1. Exception handling (catches all errors)
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // 2. Request logging
    app.UseSerilogRequestLogging();

    // 3. CORS (must be before endpoints)
    app.UseCors("AllowFrontend");

    // 4. OpenAPI documentation
    app.UseOpenApi();      // /swagger/v1/swagger.json
    app.UseSwaggerUi();    // /swagger

    // Root endpoint
    app.MapGet("/", () => Results.Ok(new
    {
        message = "Storefront API",
        timestamp = DateTime.UtcNow,
        swagger = "/swagger"
    }))
    .WithName("Root")
    .ExcludeFromDescription();

    // Map all API endpoints
    app.MapApiEndpoints();

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