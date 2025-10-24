using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.CraeteCommands;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<CreateRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<CreateRestaurantCommandHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Adding a new restaurant wirh {restaurantName}", request.Name);
            var restaurant = _mapper.Map<Restaurant>(request);
            int id = await _restaurantsRepository.AddAsync(restaurant);
            _logger.LogInformation("restaurant with id: {restaurantId} , name: {restaurantName} created successfully.", id,request.Name);
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to create restaurant due to exception");
            throw;
        }
    }
}
