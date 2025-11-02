using FluentValidation;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Commands.Users.AddToRoleCommands;

public class AddUserToRoleCommandValidator : AbstractValidator<AddUserToRoleCommand>
{
    private readonly string[] allowedRoles = [UserRoles.Admin, UserRoles.Staff, UserRoles.Owner];

    public AddUserToRoleCommandValidator()
    {
        RuleFor(x => x.Role)
            .Must(x => allowedRoles.Contains(x))
            .WithMessage($"Role must be one of the following: {string.Join(", ", allowedRoles)}");
    }
}
