using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Commands.Users.AddToRoleCommands;

public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ILogger<AddUserToRoleCommandHandler> _logger;

    public AddUserToRoleCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ILogger<AddUserToRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _roleManager.FindByNameAsync(request.Role)
                ?? throw new ResourseNotFoundException("Role", request.Role);

            var user = await _userManager.FindByIdAsync(request.Id.ToString())
                ?? throw new ResourseNotFoundException(nameof(ApplicationUser), request.Id.ToString());

            _logger.LogInformation("Adding user {UserId} to role {RoleName}", user.Id, role.Name);

            await _userManager.AddToRoleAsync(user, role.Name!);
        }
        catch(ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while adding user {UserId} to role {RoleName}", request.Id, request.Role);
            throw;
        }
    }
}
