namespace Restaurants.Application.DTOs.Restaurants;

public class GetRestaurantByIdDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool HasDelivery { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
}
