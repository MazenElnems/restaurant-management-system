namespace Restaurants.Application.DTOs.Dishes;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? KiloCalories { get; set; }
    public decimal Price { get; set; }
}
