using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Data;
using Restaurants.Infrastructure.Seaders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        return services;
    }
}
