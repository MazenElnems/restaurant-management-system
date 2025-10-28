namespace Restaurants.Application.Users;

public interface IUserContext
{
    CurrentUserIdentity? GetCurrentUser();
}
