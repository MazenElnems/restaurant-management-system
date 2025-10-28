using Restaurants.API.Extensions;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seaders;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPresentationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();

// serilog 
builder.Host.UseSerilog((context, cfg) => cfg.ReadFrom.Configuration(context.Configuration));

// swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed Default Restaurants into DB
using var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.SeedAsync();

// Configure the HTTP request pipeline.

app.UseCustomExceptionHandler();

app.UseCustomLogginMiddleware();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app .MapGroup("api/Identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
