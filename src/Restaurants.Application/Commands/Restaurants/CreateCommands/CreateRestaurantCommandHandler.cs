using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Restaurants.CreateCommands;

public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<CreateRestaurantCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<CreateRestaurantCommandHandler> logger, IUserContext userContext)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
        _userContext = userContext;
    }

    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = _userContext.GetCurrentUser()
                ?? throw new UnAuthorizedException();

            int currentUserId = currentUser.Id;
            _logger.LogInformation("User with id: {userId} is creating a new restaurant with name: {restaurantName}", currentUserId, request.Name);

            var restaurant = _mapper.Map<Restaurant>(request);
            restaurant.OwnerId = currentUserId;
            int id = await _restaurantsRepository.AddAsync(restaurant);
            _logger.LogInformation("restaurant with id: {restaurantId} , name: {restaurantName} created successfully.", id,request.Name);
            return id;
        }
        catch(UnAuthorizedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to create restaurant due to exception");
            throw;
        }
    }
}
