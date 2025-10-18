namespace Restaurants.Domain.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? KiloCalories { get; set; }

    // Foreign Keys
    public int CategoryId { get; set; }

    // Navigations
    public Category Category { get; set; }
}
