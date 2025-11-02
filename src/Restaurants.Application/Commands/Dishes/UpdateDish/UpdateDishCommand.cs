using MediatR;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Dishes.UpdateDish;

public class UpdateDishCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
    public bool IsAvailable { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
}

