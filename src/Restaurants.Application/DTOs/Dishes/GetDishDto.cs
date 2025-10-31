namespace Restaurants.Application.DTOs.Dishes;

public class GetDishDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
    public int? KiloCalories { get; set; }
    public decimal Price { get; set; }
}
