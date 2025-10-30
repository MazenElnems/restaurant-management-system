namespace Restaurants.Application.Users;

public record CurrentUserIdentity(int Id, string Email, IEnumerable<string> Roles,DateOnly? DateOfBirth, string? Nationality)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
