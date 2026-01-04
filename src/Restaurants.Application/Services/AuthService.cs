using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Restaurants.Application.Options;
using Restaurants.Application.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Restaurants.Application.Services;

public class AuthService : IAuthService
{
    private readonly JWTOptions _jwt;

    public AuthService(IOptions<JWTOptions> jwt)
    {
        _jwt = jwt.Value;
    }

    public (string, DateTime) GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var symmatricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SignInKey));
        var signInCredentials = new SigningCredentials(symmatricKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityKey = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.LifeTimeInMinutes),
            signingCredentials: signInCredentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey), jwtSecurityKey.ValidTo);
    }
}
