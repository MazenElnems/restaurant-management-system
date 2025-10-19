using AutoMapper;
using Restaurants.Application.Extensions;
using Restaurants.Application.Resaurants.DTOs;
using Restaurants.Application.Resaurants.Services.Interfaces;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Resaurants.Services;

internal class RestaurantsService : IRestaurantsService
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper mapper;

    public RestaurantsService(IRestaurantsRepository restaurantsRepository, IMapper autoMapper)
    {
        _restaurantsRepository = restaurantsRepository;
        mapper = autoMapper;
    }

    public async Task<List<AllRestaurantsDto>> GetAllRestaurantsAsync()
    {
        var restaurants = await _restaurantsRepository.GetAllAsync();
        var restaurantsDto = mapper.Map<List<AllRestaurantsDto>>(restaurants);
        return restaurantsDto;
    }

    public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = await _restaurantsRepository.GetByIdAsync(id);
        
        if(restaurant is null)
            return null;

        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
        return restaurantDto;
    }
}
