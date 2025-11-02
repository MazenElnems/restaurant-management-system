using FluentValidation;

namespace Restaurants.Application.Commands.Categories.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .MaximumLength(200)
            .WithMessage("Category name must not exceed 200 characters.");

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .When(c => !string.IsNullOrEmpty(c.Description))
            .WithMessage("Description must not exceed 500 characters.");
    }
}

