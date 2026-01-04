using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Auth;
using Restaurants.Application.Services.Interfaces;
using Restaurants.Domain.Common;
using Restaurants.Domain.Common.Errors;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System.Security.Claims;

namespace Restaurants.Application.Commands.Auth.RegisterUserCommands;

public class RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IMapper mapper,
    IAuthService authService, ILogger<RegisterUserCommandHandler> logger) : IRequestHandler<RegisterUserCommand, Result<AuthModel>>
{
    public async Task<Result<AuthModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering new user with email: {Email}", request.Email);
        if (await userManager.FindByEmailAsync(request.Email) is not null)
            return Result<AuthModel>.Failure(AuthErrors.EmailAlreadyExists);

        if(await userManager.FindByNameAsync(request.UserName) is not null)
            return Result<AuthModel>.Failure(AuthErrors.UserNameTaken);

        var user = mapper.Map<ApplicationUser>(request);

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            logger.LogError("User registration failed for email: {Email}. Errors: {Errors}", request.Email, result.Errors);
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new UserRegisterationException(errors.TrimEnd(',')); 
        }

        await userManager.AddToRoleAsync(user, UserRoles.Owner);

        var claims = new Claim[]
        {
            new( ClaimTypes.NameIdentifier, user.Id.ToString() ),
            new( ClaimTypes.Email, user.Email ),
            new( ClaimTypes.Name, user.UserName ),
            new( ClaimTypes.Role, UserRoles.Owner )
        };

        var (accessToken, accessTokenExpiration) =  authService.GenerateJwtToken(claims);

        var authModel = new AuthModel
        {
            Token = accessToken,
            ExpiresOn = accessTokenExpiration,
            Roles = [UserRoles.Owner],
            Email = user.Email,
        };

        return Result<AuthModel>.Success(authModel);
    }
}
