using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities;
using System.Security.Claims;

namespace Restaurants.API.Authorization.Claims;

public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<int>>
{
    public CustomUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<int>> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        // add custom claims here
        if (user.Nationality is not null)
            identity.AddClaim(new Claim("Nationality", user.Nationality));

        if (user.DateOfBirth != null)
            identity.AddClaim(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")));

        return identity;
    }
}
