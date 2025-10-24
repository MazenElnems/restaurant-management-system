using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.DeleteCommands;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, bool>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<DeleteRestaurantCommandHandler> _logger;

    public DeleteRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, ILogger<DeleteRestaurantCommandHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting restaurant with id: {restaurantId}", request.Id);
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id);

            if (restaurant is null)
            {
                _logger.LogWarning("Failed to delete restaurant: ID {RestaurantId} not found", request.Id);
                return false;
            }

            int rowsAffected = await _restaurantsRepository.DeleteAsync(restaurant);
            if(rowsAffected > 0)
            {
                _logger.LogInformation("restaurant ID: {RestaurantId} deleted successfully", request.Id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting restaurant ID: {RestaurantId}", request.Id);
            throw;
        }
    }
}
