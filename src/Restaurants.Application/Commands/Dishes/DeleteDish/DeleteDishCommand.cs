using MediatR;

namespace Restaurants.Application.Commands.Dishes.DeleteDish;

public class DeleteDishCommand(int id, int restaurantId) : IRequest
{
    public int Id { get; init; } = id;
    public int RestaurantId { get; init; } = restaurantId;
}

