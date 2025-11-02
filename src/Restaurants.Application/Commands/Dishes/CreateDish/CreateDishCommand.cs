using MediatR;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Dishes.CreateDish;

public class CreateDishCommand : IRequest<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int CategoryId { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
}

