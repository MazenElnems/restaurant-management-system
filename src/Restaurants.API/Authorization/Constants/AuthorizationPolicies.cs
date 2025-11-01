namespace Restaurants.API.Authorization.Constants;

public static class AuthorizationPolicies
{
    public const string HasNationalityPolicy = "HasNationalityPolicy";
    public const string AtLeast20YearsOldPolicy = "AtLeast20YearsOldPolicy";
    public const string OwnedAtLeast2Restaurant = "OwnedAtLeast2Restaurant";
}
