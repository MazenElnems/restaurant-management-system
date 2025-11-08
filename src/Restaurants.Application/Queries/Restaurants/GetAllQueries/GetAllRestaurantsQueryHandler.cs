using AutoMapper;
using MediatR;
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

    public GetAllRestaurantsQueryHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<GetAllRestaurantsQueryHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<GetAllRestaurantsDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool sortAscending = request.SortOrder == SortOrderOptions.Ascending ? true : false;
            var sortByOption = string.IsNullOrEmpty(request.SortBy) ? RestaurantSortByOptions.CreatedBy : request.SortBy;

            var (restaurants, rowsCount) = await _restaurantsRepository.GetPagedAsync(request.SearchString, sortByOption, request.PageNumber, request.PageSize, sortAscending);

            var dto = _mapper.Map<List<GetAllRestaurantsDto>>(restaurants);

            return new PagedResult<GetAllRestaurantsDto>(dto, rowsCount, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all restaurants.");
            throw;
        }
    }
}