using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Commands.Users.UpdateUsers;

public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
{
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserContext _userContext;
    private readonly ILogger<UpdateUserDetailsCommandHandler> _logger;

    public UpdateUserDetailsCommandHandler(IUserContext userContext, IUserStore<ApplicationUser> userStore, ILogger<UpdateUserDetailsCommandHandler> logger)
    {
        _userContext = userContext;
        _userStore = userStore;
        _logger = logger;
    }

    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = _userContext.GetCurrentUser()
                ?? throw new UnAuthorizedException();

            _logger.LogInformation("Updating User with ID: {UserId} Details with {@request}", currentUser.Id, request);

            var user = await _userStore.FindByIdAsync(currentUser.Id.ToString(), cancellationToken)
                ?? throw new ResourseNotFoundException(nameof(ApplicationUser), currentUser.Id.ToString());

            user.Nationality = request.Nationality;
            user.DateOfBirth = request.DateOfBirth;

            await _userStore.UpdateAsync(user, cancellationToken);
        }
        catch (UnAuthorizedException ex)
        {
            throw;
        }
        catch(ResourseNotFoundException ex)
        {
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user details for User ID: {UserId}", _userContext.GetCurrentUser()?.Id);
            throw;
        }
    }
}
