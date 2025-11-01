using MediatR;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Dishes.MoveDishesCommands;

public class MoveToCategoryCommand : IRequest
{
    [JsonIgnore]
    public int DishId { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
    public int CategoryId { get; set; }
}
