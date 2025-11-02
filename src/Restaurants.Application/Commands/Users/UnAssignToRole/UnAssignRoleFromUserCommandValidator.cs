using FluentValidation;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Commands.Users.UnAssignToRole;

public class UnAssignRoleFromUserCommandValidator : AbstractValidator<UnAssignRoleFromUserCommand>
{
    private readonly string[] allowedRoles = [UserRoles.Admin, UserRoles.Staff, UserRoles.Owner];

    public UnAssignRoleFromUserCommandValidator()
    {
        RuleFor(x => x.Role)
            .Must(x => allowedRoles.Contains(x))
            .WithMessage($"Role must be one of the following: {string.Join(", ", allowedRoles)}");
    }
}
