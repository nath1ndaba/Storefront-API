using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Storefront.Application.Common.Behaviors;
using Storefront.Application.Common.Interfaces;
using Storefront.Infrastructure.Data;
using Storefront.Infrastructure.Repositories;

namespace Storefront.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IUnitOfWork).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(IUnitOfWork).Assembly);

        // AutoMapper
        services.AddAutoMapper(typeof(IUnitOfWork).Assembly);

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("StorefrontDb"));

        // Repositories - Auto-register using Scrutor
        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationDbContext>()
            .AddClasses(classes => classes.AssignableTo(typeof(IGenericRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
