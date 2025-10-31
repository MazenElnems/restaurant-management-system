using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Commands.Users.UnAssignToRole;

public class UnAssignRoleFromUserCommandHandler : IRequestHandler<UnAssignRoleFromUserCommand>
{
    private readonly ILogger<UnAssignRoleFromUserCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UnAssignRoleFromUserCommandHandler(ILogger<UnAssignRoleFromUserCommandHandler> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(UnAssignRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(request.Role)
                ?? throw new ResourseNotFoundException("Role", request.Role);

            var user = await _userManager.FindByIdAsync(request.Id.ToString())
                ?? throw new ResourseNotFoundException(nameof(ApplicationUser), request.Id.ToString());

            _logger.LogInformation("Removing role {RoleName} from user {UserId}", role.Name, user.Id);

            await _userManager.RemoveFromRoleAsync(user, role.Name!);
        }
        catch(ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing user {UserId} from role {RoleName}", request.Id, request.Role);
        }
    }
}
