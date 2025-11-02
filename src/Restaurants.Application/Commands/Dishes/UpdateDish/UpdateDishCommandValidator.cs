using FluentValidation;

namespace Restaurants.Application.Commands.Dishes.UpdateDish;

public class UpdateDishCommandValidator : AbstractValidator<UpdateDishCommand>
{
    public UpdateDishCommandValidator()
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
    }
}

