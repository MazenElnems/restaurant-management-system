using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Restaurants.Application.DTOs.Common;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Restaurants.GetAllQueries;

public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, PagedResult<GetAllRestaurantsDto>>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllRestaurantsQueryHandler> _logger;
    private readonly IMemoryCache _cache;

    public GetAllRestaurantsQueryHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<GetAllRestaurantsQueryHandler> logger, IMemoryCache cache)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<PagedResult<GetAllRestaurantsDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var cacheKey = $"restaurant-page:{request.PageNumber}:{request.PageSize}:{request.SortOrder}:{request.SearchString}:{request.SortBy}";

            if (_cache.TryGetValue(cacheKey, out PagedResult<GetAllRestaurantsDto>? cachedResult) && cachedResult != null)
                return cachedResult;

            bool sortAscending = request.SortOrder == SortOrderOptions.Ascending ? true : false;
            var sortByOption = string.IsNullOrEmpty(request.SortBy) ? RestaurantSortByOptions.CreatedBy : request.SortBy;

            var (restaurants, rowsCount) = await _restaurantsRepository.GetPagedAsync(request.SearchString, sortByOption, request.PageNumber, request.PageSize, sortAscending);

            var dto = _mapper.Map<List<GetAllRestaurantsDto>>(restaurants);

            var result = new PagedResult<GetAllRestaurantsDto>(dto, rowsCount, request.PageNumber, request.PageSize);

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(10),       // if there is no access within 10 seconds then the cache entry expired.
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),  // fixed expireation duration.
                Size = restaurants.Count()
            });

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all restaurants.");
            throw;
        }
    }
}