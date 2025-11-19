using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Restaurants.API.Authorization.Claims;
using Restaurants.API.Authorization.Constants;
using Restaurants.API.Authorization.Requirements;
using Restaurants.API.Authorization.Requirements.RequirementHandler;
using Restaurants.API.Authorization.Services;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Data;
using Restaurants.Infrastructure.Services.DbMigrator.Interfaces;
using Restaurants.Infrastructure.Services.Seeders.Interfaces;

namespace Restaurants.API.Extensions;

public static class PresentationExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();

        // add authentication
        services.AddAuthentication();

        // add requirement authorization handlers
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, MinimumOwnedRestaurantsRequirementAuthorizationHandler>();

        // identity 
        services.AddIdentityApiEndpoints<ApplicationUser>(cfg =>
        {
            cfg.User.RequireUniqueEmail = true;
            cfg.Password.RequiredLength = 6;
            cfg.Password.RequireNonAlphanumeric = true;
            cfg.Password.RequireUppercase = true;
            cfg.Password.RequireDigit = true;
        }).AddRoles<IdentityRole<int>>()
          .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
          .AddEntityFrameworkStores<RestaurantDbContext>();

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

        // add Authorization 
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicies.HasNationalityPolicy, policy =>
            {
                policy.RequireClaim(DefaultUserClaims.Nationality);
            });

            options.AddPolicy(AuthorizationPolicies.AtLeast20YearsOldPolicy, policy =>
            {
                policy.AddRequirements(new MinimumAgeRequirement(20));
            });

            options.AddPolicy(AuthorizationPolicies.OwnedAtLeast2Restaurant, policy =>
            {
                policy.AddRequirements(new MinimumOwnedRestaurantsRequirement(2));
            });
        });

        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

        return services;
    }

    public static void InitializeDevelopmentDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<IDbMigrator>();
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();

        migrator.Migrate();
        seeder.Seed();
    }
}
