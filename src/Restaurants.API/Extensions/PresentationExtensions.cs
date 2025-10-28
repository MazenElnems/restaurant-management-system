using Microsoft.OpenApi.Models;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Data;

namespace Restaurants.API.Extensions;

public static class PresentationExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddAuthentication();

        // identity
        services.AddIdentityApiEndpoints<ApplicationUser>(cfg =>
        {
            cfg.User.RequireUniqueEmail = true;
            cfg.Password.RequiredLength = 6;
            cfg.Password.RequireNonAlphanumeric = true;
            cfg.Password.RequireUppercase = true;
            cfg.Password.RequireDigit = true;
        }).AddEntityFrameworkStores<RestaurantDbContext>();

        // swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // Add the "Authorization" header input in Swagger UI
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' followed by your token. Example: Bearer eyJhbGciOiJIUzI1..."
            });

            // Make sure Swagger uses this security scheme for all endpoints
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
