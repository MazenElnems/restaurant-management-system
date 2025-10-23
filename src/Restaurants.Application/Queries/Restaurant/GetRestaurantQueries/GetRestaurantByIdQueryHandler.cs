using AutoMapper;
using MediatR;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

public class GetRestaurantByIdQueryHandler : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto?>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;

    public GetRestaurantByIdQueryHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
    }

    public async Task<RestaurantDto?> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);
        var dto = _mapper.Map<RestaurantDto>(restaurant);
        return dto;
    }
}