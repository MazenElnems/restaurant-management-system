using Microsoft.AspNetCore.Authorization;

namespace Restaurants.API.Authorization.Requirements;

public class MinimumAgeRequirement(int age) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = age;
}
