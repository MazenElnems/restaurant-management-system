using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.API.IntegrationTest.AuthenticationHandler;
using Restaurants.API.IntegrationTest.TestServer;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Data;
using System.Net;

namespace Restaurants.API.IntegrationTest.RestaurantControllerTests;

public class GetByIdTests : IClassFixture<CustomWebApplicationFactory>
{
    private CustomWebApplicationFactory _factory;

    public GetByIdTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetById_WhenRestaurantExists_ReturnsStatusCode200Ok()
    {
        // Arrange
        var client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeCustomOptions, TestAuthHandler>("TestScheme", opt => 
                            {
                                opt.Id = "10";
                                opt.Email = "test@gmail.com";
                                opt.Role = UserRoles.Owner;
                            });
                });
            }).CreateClient();

        int id = 1;
        int ownerId = 10;
        var restaurant = new Restaurant { Id = id, Name = "Test Name", OwnerId = ownerId };

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        db.Restaurants.Add(restaurant);
        await db.SaveChangesAsync();

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_WhenRestaurantDoesNotExists_ReturnsStatusCode404NotFound()
    {
        // Arrange 
        var client = _factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeCustomOptions, TestAuthHandler>("TestScheme", opt => 
                            {
                                opt.Id = "10";
                                opt.Email = "test@gmail.com";
                                opt.Role = UserRoles.Admin; 
                            });
                });
            }).CreateClient();

        int id = 5;

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WhenUserNotAuthenticated_ReturnsStatusCode401UnAuthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        int id = 5;

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_WhenUserNotAuthorized_ReturnsStatusCode403Forbidden()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(defaultScheme: "TestScheme")
                            .AddScheme<AuthenticationSchemeCustomOptions, TestAuthHandler>("TestScheme", opt => 
                            {
                                opt.Id = "10";
                                opt.Email = "test@gmail.com";
                                opt.Role = UserRoles.Owner;
                            });
                });
            }).CreateClient();

        int id = 5;
        int ownerId = 20;
        var restaurant = new Restaurant { Id = id, Name = "Test Name" , OwnerId = ownerId };

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
        db.Restaurants.Add(restaurant);
        await db.SaveChangesAsync();

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}