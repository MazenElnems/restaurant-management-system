using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Interfaces;

namespace Restaurants.API.Authorization.Requirements.RequirementHandler;

public class MinimumOwnedRestaurantsRequirementAuthorizationHandler(IRestaurantsRepository restaurantsRepository, IUserContext userContext,
    ILogger<MinimumOwnedRestaurantsRequirementAuthorizationHandler> logger
    ) : AuthorizationHandler<MinimumOwnedRestaurantsRequirement>
{
    private readonly IRestaurantsRepository _restaurantsRepository = restaurantsRepository;
    private readonly ILogger<MinimumOwnedRestaurantsRequirementAuthorizationHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOwnedRestaurantsRequirement requirement)
    {
        var currentUser = _userContext.GetCurrentUser()
            ?? throw new UnAuthenticatedException();

        if(currentUser.IsInRole(UserRoles.Admin))
        {
            _logger.LogInformation("authorize admin user with ID: {UserId}", currentUser.Id);
            context.Succeed(requirement);
        }
        else
        {
            int numberOfOwnedRestaurants = await _restaurantsRepository.GetNumberOfOwnedRestaurants(currentUser.Id);

            if (numberOfOwnedRestaurants >= requirement.NumberOfOwnedRestaurants)
            {
                _logger.LogInformation("authorize user with ID: {UserId}", currentUser.Id);
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning("user with ID: {UserId} has no permissions", currentUser.Id);
                context.Fail();
            }
        }
    }
}