using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.API.Authorization.Requirements;
using Restaurants.API.Authorization.Requirements.RequirementHandler;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Interfaces;

namespace Restaurants.API.Tests.AuthorizationRequirement;

public class MinimumOwnedRestaurantsRequirementAuthorizationHandlerTest
{
    private readonly Mock<IRestaurantsRepository> restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
    private readonly Mock<ILogger<MinimumOwnedRestaurantsRequirementAuthorizationHandler>> loggerMock = 
        new Mock<ILogger<MinimumOwnedRestaurantsRequirementAuthorizationHandler>>();
    private readonly Mock<IUserContext> userContextMock = new Mock<IUserContext>();
    private readonly MinimumOwnedRestaurantsRequirementAuthorizationHandler handler;

    public MinimumOwnedRestaurantsRequirementAuthorizationHandlerTest()
    {
        handler = new MinimumOwnedRestaurantsRequirementAuthorizationHandler(restaurantsRepositoryMock.Object,
            userContextMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenUserIsAdmin_ShouldSucceed()
    {
        // Arrange
        var currentUserIdentity = new CurrentUserIdentity(1, "test@gmail.com", [UserRoles.Admin],null,null);

        userContextMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUserIdentity);

        var requirement = new MinimumOwnedRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue(); 
    }

    [Fact]
    public async Task HandleRequirementAsync_UserOwnedValidNumberOfRestaurants_ShouldSucceed()
    {
        // Arrange
        var currentUserIdentity = new CurrentUserIdentity(1, "test@gmail.com", [], null, null);

        userContextMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUserIdentity);

        restaurantsRepositoryMock.Setup(x => x.GetNumberOfOwnedRestaurants(currentUserIdentity.Id))
            .ReturnsAsync(2);

        var requirement = new MinimumOwnedRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserNotAuthenticated_ThrowsUnAuthenticatedException()
    {
        // Arrange
        userContextMock.Setup(x => x.GetCurrentUser())
            .Returns((CurrentUserIdentity)null!);

        var requirement = new MinimumOwnedRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        var func = async () =>  await handler.HandleAsync(context);

        // Assert
        await func.Should().ThrowAsync<UnAuthenticatedException>();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserOwnedInValidNumberOfRestaurants_ShouldSucceed()
    {
        // Arrange
        var currentUserIdentity = new CurrentUserIdentity(1, "test@gmail.com", [], null, null);

        userContextMock.Setup(x => x.GetCurrentUser())
            .Returns(currentUserIdentity);

        restaurantsRepositoryMock.Setup(x => x.GetNumberOfOwnedRestaurants(currentUserIdentity.Id))
            .ReturnsAsync(1);

        var requirement = new MinimumOwnedRestaurantsRequirement(2);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }
}
