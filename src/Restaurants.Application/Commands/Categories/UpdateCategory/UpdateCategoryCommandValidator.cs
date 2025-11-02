using FluentValidation;

namespace Restaurants.Application.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .MaximumLength(100)
            .WithMessage("Category name must not exceed 100 characters.");

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .When(c => !string.IsNullOrEmpty(c.Description))
            .WithMessage("Description must not exceed 500 characters.");
    }
}

