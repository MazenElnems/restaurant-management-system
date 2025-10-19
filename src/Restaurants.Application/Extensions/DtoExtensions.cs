using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Resaurants.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Extensions;

internal static class DtoExtensions
{
    public static RestaurantDto? ToRestaurantDto(this Restaurant? restaurant)
    {
        if (restaurant == null)
            return null;

        return new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            HasDelivery = restaurant.HasDelivery,
            City = restaurant.Address.City,
            Street = restaurant.Address.Street,
            PostalCode = restaurant.Address.PostalCode,
            Categories = restaurant.Categories.Select(c => c.ToCategoryDto()).ToList()
        };
    }

    public static AllRestaurantsDto ToAllRestaurantsDto(this Restaurant restaurant)
    {
        return new AllRestaurantsDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            HasDelivery = restaurant.HasDelivery,
            City = restaurant.Address.City,
            Street = restaurant.Address.Street,
            PostalCode = restaurant.Address.PostalCode,
        };
    }

    public static CategoryDto ToCategoryDto(this Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Dishes = category.Dishes.Select(d => d.ToDishDto()).ToList()
        };
    }

    public static DishDto ToDishDto(this Dish dish)
    {
        return new DishDto
        {
            Id = dish.Id,
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price,
            KiloCalories = dish.KiloCalories
        };
    }
}
