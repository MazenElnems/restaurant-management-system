using MediatR;

namespace Restaurants.Application.Commands.Restaurants.DeleteCommands;

public class DeleteRestaurantCommand(int id) : IRequest
{
    public int Id { get; private set; } = id;
}
