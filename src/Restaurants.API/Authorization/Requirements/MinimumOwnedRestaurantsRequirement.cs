using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Data;

namespace Restaurants.API.Authorization.Requirements;

public class MinimumOwnedRestaurantsRequirement(int minimumNumber) : IAuthorizationRequirement
{
    public int NumberOfOwnedRestaurants { get; set; } = minimumNumber;
}

public class MinimumOwnedRestaurantsRequirementAuthorizationHandler(RestaurantDbContext db, IUserContext userContext,
    ILogger<MinimumOwnedRestaurantsRequirementAuthorizationHandler> logger
    ) : AuthorizationHandler<MinimumOwnedRestaurantsRequirement>
{
    private readonly RestaurantDbContext _db = db;
    private readonly ILogger<MinimumOwnedRestaurantsRequirementAuthorizationHandler> _logger = logger;
    private readonly IUserContext _userContext = userContext;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOwnedRestaurantsRequirement requirement)
    {
        var currentUser = _userContext.GetCurrentUser()
            ?? throw new UnAuthorizedException();

        if(currentUser.IsInRole(UserRoles.Admin))
        {
            _logger.LogInformation("authorize admin user with ID: {UserId}", currentUser.Id);
            context.Succeed(requirement);
            return Task.CompletedTask;
        }    

        int numberOfOwnedRestaurants = _db.Restaurants.Count(r => r.OwnerId == currentUser.Id);

        if(numberOfOwnedRestaurants >= requirement.NumberOfOwnedRestaurants)
        {
            _logger.LogInformation("authorize user with ID: {UserId}", currentUser.Id);
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning("user with ID: {UserId} has no permissions", currentUser.Id);
            context.Fail();
        }
        return Task.CompletedTask;
    }
}