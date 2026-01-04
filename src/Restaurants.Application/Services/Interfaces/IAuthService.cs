using System.Security.Claims;

namespace Restaurants.Application.Services.Interfaces;

public interface IAuthService
{
    (string, DateTime) GenerateJwtToken(IEnumerable<Claim> claims);
}