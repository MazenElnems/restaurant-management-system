using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.DTOs.Auth;
using Restaurants.Application.Services.Interfaces;
using Restaurants.Domain.Common;
using Restaurants.Domain.Common.Claims;
using Restaurants.Domain.Common.Errors;
using Restaurants.Domain.Entities;
using System.Security.Claims;

namespace Restaurants.Application.Commands.Auth.GetTokenCommands;

public class GetTokenCommandHandler : IRequestHandler<GetTokenCommand, Result<AuthModel>>
{
    private readonly ILogger<GetTokenCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;

    public GetTokenCommandHandler(UserManager<ApplicationUser> userManager, ILogger<GetTokenCommandHandler> logger, IAuthService authService)
    {
        _userManager = userManager;
        _logger = logger;
        _authService = authService;
    }

    public async Task<Result<AuthModel>> Handle(GetTokenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation ("Handling GetTokenCommand for email: {Email}", request.Email);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if(user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            _logger.LogWarning("Invalid login attempts for email: {Email}", request.Email);
            return Result<AuthModel>.Failure(AuthErrors.InvalidEmailOrPassword);
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Email, user.Email),
            new (DefaultUserClaims.DateOfBirth, user.DateOfBirth.ToString()),
            new (DefaultUserClaims.Nationality, user.Nationality)
        }.Union(userClaims)
         .Union(roleClaims);

        var (token, expiresOn) = _authService.GenerateJwtToken(claims);

        var authModel = new AuthModel
        {
            Token = token,
            ExpiresOn = expiresOn,
            Email = user.Email,
            Roles = roles.ToList()
        };

        return Result<AuthModel>.Success(authModel);
    }
}
