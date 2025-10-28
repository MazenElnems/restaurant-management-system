using MediatR;

namespace Restaurants.Application.Commands.Users.UpdateUsers;

public class UpdateUserDetailsCommand : IRequest
{
    public string? Nationality { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
