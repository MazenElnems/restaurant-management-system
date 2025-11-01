namespace Restaurants.Domain.Entities;

public class Restaurant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool HasDelivery { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }
    public Address Address { get; set; }

    // Foreign Keys
    public int OwnerId { get; set; }

    // Navigations
    public virtual Owner Owner { get; set; }
    public virtual List<Category> Categories { get; set; } = new List<Category>();
}
