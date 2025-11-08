using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;

namespace Restaurants.API.Authorization.Requirements.RequirementHandler;

public class MinimumAgeRequirementAuthorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    private readonly ILogger<MinimumAgeRequirementAuthorizationHandler> _logger;
    private readonly IUserContext _userContext;

    public MinimumAgeRequirementAuthorizationHandler(IUserContext userContext, ILogger<MinimumAgeRequirementAuthorizationHandler> logger)
    {
        _userContext = userContext;
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = _userContext.GetCurrentUser()
            ?? throw new UnAuthorizedException();

        var dateOfBirth = currentUser.DateOfBirth;

        if (dateOfBirth == null)
        {
            context.Fail();
            _logger.LogWarning("Authorization failed for user {UserId}. Date of birth is not provided.", currentUser.Id);
            return Task.CompletedTask;
        }

        int age = DateTime.UtcNow.Year - dateOfBirth.Value.Year;

        if(age >= requirement.MinimumAge)
        {
            _logger.LogInformation("Authorization succeeded for user {UserId} with age {Age}", currentUser.Id, age);
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
            _logger.LogWarning("Authorization failed for user {UserId} with age {Age}. Minimum required age is {MinimumAge}", currentUser.Id, age, requirement.MinimumAge);
        }
        return Task.CompletedTask;

    }
}