using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Restaurants.GetRestaurantQueries;

public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, GetRestaurantByIdDto>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRestaurantByIdQueryHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;
    private readonly IMemoryCache _cache;


    public GetRestaurantByIdQueryHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<GetRestaurantByIdQueryHandler> logger, IRestaurantAuthorizationService authorizationService, IMemoryCache cache)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _cache = cache;
    }

    public async Task<GetRestaurantByIdDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"restaurant_{request.Id}";
            
            if (_cache.TryGetValue(cacheKey, out GetRestaurantByIdDto? cachedDto) && cachedDto != null)
                return cachedDto;

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException("Restaurant", request.Id.ToString());

            if (!_authorizationService.Authorize(restaurant,RestaurantOperation.Read))
                throw new UnAuthorizedException("You are not authorized to access this restaurant.");

            var dto = _mapper.Map<GetRestaurantByIdDto>(restaurant);

            _cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
            {
                Size = 1,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });

            return dto;
        }
        catch(UnAuthorizedException ex)
        {
            throw;
        }
        catch (ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting restaurant by id {RestaurantId}", request.Id);
            throw;
        }
    }
}