using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Commands.Dishes.UpdateDish;

public class UpdateDishCommand : IRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
    public bool IsAvailable { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [JsonIgnore]
    public int RestaurantId { get; set; }
}

