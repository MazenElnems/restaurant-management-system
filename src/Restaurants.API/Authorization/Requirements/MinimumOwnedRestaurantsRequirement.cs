using Microsoft.AspNetCore.Authorization;

namespace Restaurants.API.Authorization.Requirements;

public class MinimumOwnedRestaurantsRequirement(int minimumNumber) : IAuthorizationRequirement
{
    public int NumberOfOwnedRestaurants { get; set; } = minimumNumber;
}
