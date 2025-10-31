using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Dishes.CreateDish;

public class CreateDishCommand : IRequest<int>
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
    public bool IsAvailable { get; set; } = true;
    [Required]
    public int CategoryId { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
}

