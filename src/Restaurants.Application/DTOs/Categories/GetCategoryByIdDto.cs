using Restaurants.Application.DTOs.Dishes;

namespace Restaurants.Application.DTOs.Categories;

public class GetCategoryByIdDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<int> DishIds { get; set; }
}
