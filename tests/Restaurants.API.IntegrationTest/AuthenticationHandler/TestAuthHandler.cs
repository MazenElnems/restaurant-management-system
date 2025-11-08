using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Restaurants.API.IntegrationTest.AuthenticationHandler;

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeCustomOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeCustomOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier,Options.Id),
            new(ClaimTypes.Email,Options.Email),
            new(ClaimTypes.Role, Options.Role),
        };

        var claimIdentity = new ClaimsIdentity(claims,"Test");
        var claimsPrincipal = new ClaimsPrincipal(claimIdentity);

        var ticket = new AuthenticationTicket(claimsPrincipal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}

public class AuthenticationSchemeCustomOptions : AuthenticationSchemeOptions
{
    public string Id { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
}
