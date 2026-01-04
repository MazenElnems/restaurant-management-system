using MediatR;
using Restaurants.Application.DTOs.Auth;
using Restaurants.Domain.Common;

namespace Restaurants.Application.Commands.Auth.RegisterUserCommands;

public class RegisterUserCommand : IRequest<Result<AuthModel>>
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string? Country { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
