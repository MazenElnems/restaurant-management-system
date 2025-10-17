namespace Restaurants.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    // Foreign Keys
    public int RestaurantId { get; set; }

    // Navigations
    public Restaurant Restaurant { get; set; }
    public List<Dish> Dishes { get; set; } = new List<Dish>();
}
