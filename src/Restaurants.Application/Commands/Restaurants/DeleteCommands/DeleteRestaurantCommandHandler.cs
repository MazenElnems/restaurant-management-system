using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Restaurants.DeleteCommands;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<DeleteRestaurantCommandHandler> _logger;

    public DeleteRestaurantCommandHandler(IRestaurantsRepository restaurantsRepository, ILogger<DeleteRestaurantCommandHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting restaurant with id: {restaurantId}", request.Id);
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException("Restaurant", request.Id.ToString());

            await _restaurantsRepository.DeleteAsync(restaurant);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting restaurant ID: {RestaurantId}", request.Id);
            throw;
        }
    }
}
