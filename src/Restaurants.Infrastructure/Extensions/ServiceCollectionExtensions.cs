using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Data;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Services.DbMigrator;
using Restaurants.Infrastructure.Services.DbMigrator.Interfaces;
using Restaurants.Infrastructure.Services.Seeders;
using Restaurants.Infrastructure.Services.Seeders.Interfaces;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantDbContext>(options => 
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .UseLazyLoadingProxies()
                   .EnableSensitiveDataLogging();
        });
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped<IDbMigrator, DbMigrator>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<ICategoriesRepository, CategoriesRepository>();
        return services;
    }
}
