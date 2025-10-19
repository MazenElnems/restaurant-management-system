using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Resaurants.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<DishDto> Dishes { get; set; } 
}
