using Restaurants.Domain.Entities;

namespace Restaurants.Application.DTOs.Dishes;

public class GetAllDishesDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }
    public bool IsAvailable { get; set; }
    public int CategoryId { get; set; }
}
