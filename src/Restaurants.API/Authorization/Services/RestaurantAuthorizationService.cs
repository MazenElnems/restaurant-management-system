using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.API.Authorization.Services;

public class RestaurantAuthorizationService : IRestaurantAuthorizationService
{
    private readonly IUserContext _userContext;
    private readonly ILogger<RestaurantAuthorizationService> _logger;

    public RestaurantAuthorizationService(IUserContext userContext, ILogger<RestaurantAuthorizationService> logger)
    {
        _userContext = userContext;
        _logger = logger;
    }

    public bool Authorize(Restaurant restaurant, RestaurantOperation operation)
    {
        var currentUser = _userContext.GetCurrentUser()
            ?? throw new UnAuthorizedException();

        _logger.LogInformation("Authorizing user {UserId} for operation {Operation} on restaurant {RestaurantId}",
            currentUser.Id, operation, restaurant.Id);

        if (operation == RestaurantOperation.Create)
            return true;

        if(operation == RestaurantOperation.Update && currentUser.Id == restaurant.OwnerId)
            return true;

        if(operation == RestaurantOperation.Delete && (currentUser.Id == restaurant.OwnerId || currentUser.IsInRole(UserRoles.Admin)))
            return true;

        if(operation == RestaurantOperation.Read && (currentUser.Id == restaurant.OwnerId || currentUser.IsInRole(UserRoles.Admin)))
            return true;

        _logger.LogWarning("Authorization failed for user {UserId} on operation {Operation} for restaurant {RestaurantId}",
            currentUser.Id, operation, restaurant.Id);

        return false;
    }
}
