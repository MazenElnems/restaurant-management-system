using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Dishes.DeleteDish;

public class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand>
{
    private readonly IDishesRepository _dishesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<DeleteDishCommandHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public DeleteDishCommandHandler(IDishesRepository dishesRepository, IRestaurantsRepository restaurantsRepository, ILogger<DeleteDishCommandHandler> logger, IRestaurantAuthorizationService authorizationService)
    {
        _dishesRepository = dishesRepository;
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to delete dish from this restaurant.");

            var dish = await _dishesRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Dish), request.Id.ToString());

            _logger.LogInformation("Deleting Dish: {@Dish}", dish);

            await _dishesRepository.DeleteAsync(dish);
            _logger.LogInformation("Dish: {@Dish} deleted successfully.", dish);
        }
        catch(UnAuthorizedException ex)
        {
            throw;
        }
        catch (ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting dish with Id {DishId}", request.Id);
            throw;
        }
    }
}

