using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, GetRestaurantByIdDto>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetRestaurantByIdQueryHandler> _logger;

    public GetRestaurantByIdQueryHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<GetRestaurantByIdQueryHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GetRestaurantByIdDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException("Restaurant", request.Id.ToString());

            var dto = _mapper.Map<GetRestaurantByIdDto>(restaurant);
            return dto;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting restaurant by id {RestaurantId}", request.Id);
            throw;
        }
    }
}