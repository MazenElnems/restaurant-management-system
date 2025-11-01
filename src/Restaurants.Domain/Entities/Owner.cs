namespace Restaurants.Domain.Entities;

public class Owner : ApplicationUser
{
    // Navigations
    public virtual List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
