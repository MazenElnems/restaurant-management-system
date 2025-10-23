using AutoMapper;
using MediatR;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.CraeteCommands;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;

    public CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = _mapper.Map<Restaurant>(request);
        int id = await _restaurantsRepository.AddAsync(restaurant);
        return id;
    }
}
