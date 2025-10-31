using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Dishes.GetByIdQueries;

public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, GetDishDto>
{

    private readonly IRestaurantAuthorizationService _authorizationService;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetDishByIdQueryHandler> _logger;

    public GetDishByIdQueryHandler(IDishesRepository dishesRepository, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService authorizationService, IMapper mapper, ILogger<GetDishByIdQueryHandler> logger)
    {
        _dishesRepository = dishesRepository;
        _restaurantsRepository = restaurantsRepository;
        _authorizationService = authorizationService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetDishDto> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!_authorizationService.Authorize(restaurant, RestaurantOperation.Read))
                throw new UnAuthorizedException("You are not authorized to access this restaurant.");

            _logger.LogInformation("Fetching dish with id {DishId} for restaurant {RestaurantId}", request.Id, request.RestaurantId);

            var dish = await _dishesRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Dish), request.Id.ToString());

            var dto = _mapper.Map<GetDishDto>(dish);
            return dto;
        }
        catch(ResourseNotFoundException ex)
        {
            throw;
        }
        catch (UnAuthorizedException ex)
        {
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching dish with id {DishId} for restaurant {RestaurantId}", request.Id, request.RestaurantId);
            throw;  
        }
    }
}
