using MediatR;
using Restaurants.Application.DTOs.Auth;
using Restaurants.Domain.Common;

namespace Restaurants.Application.Commands.Auth.GetTokenCommands;

public class GetTokenCommand : IRequest<Result<AuthModel>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
