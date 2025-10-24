using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, List<AllRestaurantsDto>>
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

    public async Task<List<AllRestaurantsDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurants = await _restaurantsRepository.GetAllAsync();
            var dto = _mapper.Map<List<AllRestaurantsDto>>(restaurants);
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all restaurants.");
            throw;
        }
    }
}