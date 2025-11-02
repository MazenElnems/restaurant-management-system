using FluentValidation;

namespace Restaurants.Application.Commands.Dishes.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(d => d.Name)
            .MaximumLength(200)
            .WithMessage("Dish name must not exceed 200 characters.");

        RuleFor(d => d.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(d => d.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .When(d => d.KiloCalories.HasValue)
            .WithMessage("KiloCalories must be greater than or equal to 0.");

        RuleFor(d => d.CategoryId)
            .GreaterThan(0)
            .WithMessage("Valid CategoryId is required.");
    }
}

