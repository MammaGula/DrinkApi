using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DrinkApi.Data;
using DrinkApi.Data.Interfaces;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Extensions;

public static class ServiceCollectionExtensions
{
    // Register infrastructure concerns: DbContext and repositories
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database - Scoped by default
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IDrinkRepository, DrinkRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    // Register application services (business logic)
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDrinkService, DrinkApi.Services.DrinkService>();
        services.AddScoped<IOrderService, DrinkApi.Services.OrderService>();

        return services;
    }
}
