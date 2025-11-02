using FluentValidation;

namespace Restaurants.Application.Commands.Dishes.MoveDishesCommands;

public class MoveToCategoryCommandValidator : AbstractValidator<MoveToCategoryCommand>
{
    public MoveToCategoryCommandValidator()
    {
        RuleFor(m => m.CategoryId)
            .GreaterThan(0)
            .WithMessage("Valid CategoryId is required.");
    }
}

