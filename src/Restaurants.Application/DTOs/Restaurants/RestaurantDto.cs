using Restaurants.Application.DTOs.Categories;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.DTOs.Restaurants;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool HasDelivery { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public List<CategoryDto> Categories { get; set; }
}
