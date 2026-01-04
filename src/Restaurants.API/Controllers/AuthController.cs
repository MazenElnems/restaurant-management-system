using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Commands.Auth.GetTokenCommands;
using Restaurants.Application.Commands.Auth.RegisterUserCommands;

namespace Restaurants.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("token")]
    public async Task<IActionResult> Login(GetTokenCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return Unauthorized(new
            {
                Errors = new[] { result.Error }
            });

        return Ok(new
        {
            result.Value.Email,
            result.Value.Token,
            result.Value.ExpiresOn,
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return BadRequest(new
            {
                Errors = new [] { result.Error }
            });

        return Ok(new
        {
            result.Value.Email,
            result.Value.Token,
            result.Value.ExpiresOn,
        });
    }
}
