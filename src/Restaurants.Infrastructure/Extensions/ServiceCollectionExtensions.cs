using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.RepositoryInterfaces;
using Restaurants.Infrastructure.Data;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seaders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();

        return services;
    }
}
