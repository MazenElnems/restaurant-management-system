using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.Application.Users;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserIdentity? GetCurrentUser()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if(httpContext.User.Identity is null || httpContext.User.Identity.IsAuthenticated is false)
        {
            return null;
        }

        var id = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        var roles = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(c => c.Value);

        return new CurrentUserIdentity(Convert.ToInt32(id), email!, roles);
    }
}
