using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Dishes.GetDishesQueries;

public class GetAllDishesQueryHandler : IRequestHandler<GetAllDishesQuery, List<GetAllDishesDto>>
{
    private readonly IRestaurantAuthorizationService _authorizationService;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllDishesQueryHandler> _logger;
    private readonly IMemoryCache _cache;

    public GetAllDishesQueryHandler(IDishesRepository dishesRepository, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService authorizationService, IMapper mapper, ILogger<GetAllDishesQueryHandler> logger, IMemoryCache cache)
    {
        _dishesRepository = dishesRepository;
        _restaurantsRepository = restaurantsRepository;
        _authorizationService = authorizationService;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<GetAllDishesDto>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"restaurant_dishes_{request.RestaurantId}";

            if(_cache.TryGetValue(cacheKey, out List<GetAllDishesDto>? cachedDto) && cachedDto != null)
                return cachedDto;

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Read))
                throw new UnAuthorizedException("You are not authorized to access this restaurant.");

            _logger.LogInformation("Fetching all dishes for restaurant with ID {RestaurantId}", request.RestaurantId);

            var dishes = await _dishesRepository.GetAllByRestaurantIdAsync(request.RestaurantId);
            var dto = _mapper.Map<List<GetAllDishesDto>>(dishes);

            _cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                Size = dto.Count()
            });

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching dishes for restaurant with ID {RestaurantId}", request.RestaurantId);
            throw;
        }
    }
}