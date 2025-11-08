using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Data;

namespace Restaurants.API.IntegrationTest.TestServer;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var efServices = services.Where(descriptor =>
                descriptor.ServiceType.ToString().StartsWith("Microsoft.EntityFrameworkCore") ||
                descriptor.ServiceType == typeof(RestaurantDbContext) ||
                descriptor.ServiceType == typeof(DbContextOptions) ||
                descriptor.ServiceType == typeof(DbContextOptions<RestaurantDbContext>))
                .ToList();

            foreach (var efService in efServices)
            {
                services.Remove(efService);
            }

            // Add InMemory
            services.AddDbContext<RestaurantDbContext>(options =>
            {
                options.UseInMemoryDatabase("RestaurantInMemoryDB")
                       .UseLazyLoadingProxies()
                       .EnableSensitiveDataLogging();
            });
        });
    }
}
