using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.API.IntegrationTest.AuthenticationHandler;
using Restaurants.API.IntegrationTest.TestServer;
using Restaurants.Domain.Constants;
using System.Net;

namespace Restaurants.API.IntegrationTest.RestaurantControllerTests;

public class GetAllTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public GetAllTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAll_WhenUserIsInAdminRole_ReturnsStatusCode200Ok()
    {
        // Arrnage
        var client = _factory.WithWebHostBuilder(builder =>
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

        int pageNo = 1;
        int pageSize = 5;

        // Act
        var response = await client.GetAsync($"/api/restaurants?pageNumber={pageNo}&pageSize={pageSize}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_WhenOwnedAtLeast2RestaurantRequirementFaild_ReturnsStatusCode403Forbidden()
    {
        // Arrnage
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

        int pageNo = 1;
        int pageSize = 5;

        // Act
        var response = await client.GetAsync($"/api/restaurants?pageNumber={pageNo}&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetAll_InvalidRequest_ReturnsStatusCode400BadRequest()
    {
        // Arrnage
        var client = _factory.WithWebHostBuilder(builder =>
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

        // Act
        var response = await client.GetAsync($"/api/restaurants");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAll_UnAuthenticatedUser_ReturnsStatusCode401UnAuthorized()
    {
        // Arrnage
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/restaurants?pageNumber=1&pageSize=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
