using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Commands.Restaurants.UpdateCommands;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Tests.Handlers;

public class UpdateRestaurantCommandHandlerTest
{
    private Mock<ILogger<UpdateRestaurantCommandHandler>> loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
    private Mock<IRestaurantsRepository> restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
    private Mock<IRestaurantAuthorizationService> restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
    private IFixture fixture = new Fixture();
    private UpdateRestaurantCommandHandler handler;

    public UpdateRestaurantCommandHandlerTest()
    {
        handler = new UpdateRestaurantCommandHandler(restaurantRepositoryMock.Object,
            loggerMock.Object, restaurantAuthorizationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesRestaurantSuccessfully()
    {
        // Arrange
        var restaurantId = 1;
        var command = fixture.Build<UpdateRestaurantCommand>()
            .With(x => x.Id, restaurantId)
            .Create();

        var restaurant = new Restaurant { Id = restaurantId };

        restaurantRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurant);

        restaurantAuthorizationServiceMock
            .Setup(x => x.Authorize(It.IsAny<Restaurant>(), RestaurantOperation.Update))
            .Returns(true);

        restaurantRepositoryMock.Setup(x => x.CommitAsync())
            .ReturnsAsync(1);

        // Act
        await handler.Handle(command,CancellationToken.None);

        // Assert
        restaurantRepositoryMock.Verify(x => x.CommitAsync(), Times.Once());
        
        restaurant.Id.Should().Be(command.Id);
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address.City.Should().Be(command.City);
        restaurant.Address.Street.Should().Be(command.Street);
        restaurant.Address.PostalCode.Should().Be(command.PostalCode);
    }

    [Fact]
    public async Task Handle_WhenRestaurantDoesNotExist_ThrowResourceNotFoundException()
    {
        // Arrange
        var command = fixture.Build<UpdateRestaurantCommand>()
            .Create();

        restaurantRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Restaurant)null!);

        // Act
        var func = () => handler.Handle(command,CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<ResourseNotFoundException>();
    }

    [Fact]
    public async Task Handle_UnAuthorizedUser_ThrowUnAuthorizedException()
    {
        // Arrange
        var command = fixture.Build<UpdateRestaurantCommand>()
            .Create();

        var restaurant = new Restaurant();

        restaurantRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(restaurant);

        restaurantAuthorizationServiceMock.Setup(x => x.Authorize(restaurant, RestaurantOperation.Update))
            .Returns(false);

        // Act
        var func = () => handler.Handle(command,CancellationToken.None);

        // Assert
        await func.Should().ThrowAsync<UnAuthorizedException>();
    }
}
