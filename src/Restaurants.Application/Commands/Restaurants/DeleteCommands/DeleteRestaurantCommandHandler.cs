using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Restaurants.DeleteCommands;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<DeleteRestaurantCommandHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public DeleteRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, ILogger<DeleteRestaurantCommandHandler> logger, IRestaurantAuthorizationService authorizationService)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting restaurant with id: {restaurantId}", request.Id);
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException("Restaurant", request.Id.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Delete))
                throw new UnAuthorizedException("User is not authorized to delete this restaurant.");

            await _restaurantsRepository.DeleteAsync(restaurant);

        }
        catch (UnAuthorizedException ex)
        {
            throw;
        }
        catch (ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting restaurant ID: {RestaurantId}", request.Id);
            throw;
        }
    }
}
