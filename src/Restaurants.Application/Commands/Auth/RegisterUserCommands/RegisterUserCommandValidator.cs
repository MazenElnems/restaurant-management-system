using FluentValidation;

namespace Restaurants.Application.Commands.Auth.RegisterUserCommands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}
