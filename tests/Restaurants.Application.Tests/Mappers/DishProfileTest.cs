using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Restaurants.Application.Commands.Dishes.CreateDish;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Application.Queries.Dishes;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests.Mappers;

public class DishProfileTest
{
    private IMapper mapper;

    public DishProfileTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DishProfile>();
        }, NullLoggerFactory.Instance);

        mapper = configuration.CreateMapper();
    }

    [Fact]
    public void Map_ForCreateDishCommandToDish_MapsCorrectly()
    {
        // Arrange
        var command = new CreateDishCommand
        {
            Name = "Test Name",
            Description = "Test Description",
            IsAvailable = true,
            KiloCalories = 1,
            CategoryId = 1,
            Price = 1.0m,
            RestaurantId = 1,
        };

        // Act
        var dish = mapper.Map<Dish>(command);

        // Assert
        dish.Should().NotBeNull();
        dish.Id.Should().Be(0);
        dish.Name.Should().Be("Test Name");
        dish.Description.Should().Be("Test Description");
        dish.IsAvailable.Should().BeTrue();
        dish.KiloCalories.Should().Be(1);
        dish.CategoryId.Should().Be(1);
        dish.Price.Should().Be(1.0m);
    }

    [Fact]
    public void Map_ForDishToGetAllDishesDto_MapsCorrectly()
    {
        // Arrange
        var dish = new Dish
        {
            Id = 1,
            Name = "Dish Name Test",
            Price = 1,
            KiloCalories = 1,
            IsAvailable = true,
            CategoryId = 1
        };

        // Act
        var dto = mapper.Map<GetAllDishesDto>(dish);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Dish Name Test");
        dto.IsAvailable.Should().BeTrue();
        dto.KiloCalories.Should().Be(1);
        dto.CategoryId.Should().Be(1);
        dto.Price.Should().Be(1);
    }

    [Fact]
    public void Map_ForDishToGetDishDto_MapsCorrectly()
    {
        // Arrange
        var dish = new Dish
        {
            Id = 1,
            Name = "Dish Name Test",
            Description = "Dish Description Test",
            Price = 1,
            KiloCalories = 1,
            IsAvailable = true,
            CategoryId = 1
        };

        // Act
        var dto = mapper.Map<GetDishDto>(dish);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Dish Name Test");
        dto.Description.Should().Be("Dish Description Test");
        dto.IsAvailable.Should().BeTrue();
        dto.KiloCalories.Should().Be(1);
        dto.Price.Should().Be(1);
    }
}
