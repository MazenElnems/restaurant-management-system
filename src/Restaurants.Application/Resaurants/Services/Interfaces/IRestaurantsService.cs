using Restaurants.Application.Resaurants.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Resaurants.Services.Interfaces;

public interface IRestaurantsService
{
    Task<List<AllRestaurantsDto>> GetAllRestaurantsAsync();
    Task<RestaurantDto?> GetRestaurantByIdAsync(int id);
    Task<int> CreateRestaurantAsync(CreateRestaurantDto dto);
}
