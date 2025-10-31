using MediatR;

namespace Restaurants.Application.Commands.Dishes.MoveDishesCommands;

public class MoveToCategoryCommand : IRequest
{
    public int DishId { get; set; }
    public int RestaurantId { get; set; }
    public int CategoryId { get; set; }
}
