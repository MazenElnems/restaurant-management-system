using Restaurants.API.Extensions;
using Restaurants.API.Middleware;
using Restaurants.API.Middlewares;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Serilog;

try
{
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
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.InitializeDevelopmentDatabase();
    }

    // Configure the HTTP request pipeline.

    app.UseCustomExceptionHandler();

    app.UseCustomLogginMiddleware();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseSerilogRequestLogging();

    app.MapGroup("api/Identity")
        .WithTags("Identity")
        .MapIdentityApi<ApplicationUser>();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}

public partial class Program { }
