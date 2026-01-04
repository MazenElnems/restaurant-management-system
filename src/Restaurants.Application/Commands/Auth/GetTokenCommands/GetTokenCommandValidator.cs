using FluentValidation;

namespace Restaurants.Application.Commands.Auth.GetTokenCommands;

public class GetTokenCommandValidator : AbstractValidator<GetTokenCommand>
{
    public GetTokenCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
