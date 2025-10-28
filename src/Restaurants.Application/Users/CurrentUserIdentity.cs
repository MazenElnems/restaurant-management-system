namespace Restaurants.Application.Users;

public record CurrentUserIdentity(int Id, string Email, IEnumerable<string> Roles)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
