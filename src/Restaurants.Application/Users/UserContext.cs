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

        var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        var roles = httpContext.User.FindAll(ClaimTypes.Role).Select(c => c.Value);
        var dateOfBirthClaim = httpContext.User.FindFirst("DateOfBirth")?.Value;
        var nationality = httpContext.User.FindFirst("Nationality")?.Value;

        DateOnly? dateOfBirthClaimParsed = dateOfBirthClaim is not null ?
            DateOnly.Parse(dateOfBirthClaim) :
            null;
   
        return new CurrentUserIdentity(Convert.ToInt32(id), email!, roles, dateOfBirthClaimParsed, nationality);
    }
}
