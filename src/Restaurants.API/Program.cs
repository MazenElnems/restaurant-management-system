using Restaurants.API.Services;
using Restaurants.API.Services.Interfaces;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seaders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IWeatherForecastService,WeatherForecastService>();

builder.Services.AddInfrastructureServices(builder.Configuration);


var app = builder.Build();

// Seed Default Restaurants into DB

using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.SeedAsync();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
