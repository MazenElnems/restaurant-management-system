using AutoFixture;
using Restaurants.Application.Commands.Dishes.CreateDish;
using FluentValidation.TestHelper;

namespace Restaurants.Application.Tests.Validators;

public class CreateDishCommandValidatorTest 
{
    private CreateDishCommandValidator validator;

    public CreateDishCommandValidatorTest()
    {
        validator = new CreateDishCommandValidator();
    }

    [Fact]
    public void Validator_ValidCreateDishCommand_ShouldNotHaveAnyValidationErrors()
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
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_InValidCreateDishCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateDishCommand
        {
            Name = new string('a',201),
            CategoryId = 0,
            Price = 0,
            KiloCalories = -1,
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
        result.ShouldHaveValidationErrorFor(x => x.Price);
        result.ShouldHaveValidationErrorFor(x => x.KiloCalories);
    }
}
