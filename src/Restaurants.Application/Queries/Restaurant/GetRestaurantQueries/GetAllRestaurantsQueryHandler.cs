using AutoMapper;
using MediatR;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, List<AllRestaurantsDto>>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;

    public GetAllRestaurantsQueryHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
    }

    public async Task<List<AllRestaurantsDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantsRepository.GetAllAsync();
        var dto = _mapper.Map<List<AllRestaurantsDto>>(restaurants);
        return dto;
    }
}