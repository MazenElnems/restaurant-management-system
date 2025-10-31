using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Dishes.MoveDishesCommands;

public class MoveToCategoryCommandHandler : IRequestHandler<MoveToCategoryCommand>
{
    private readonly IRestaurantAuthorizationService _authorizationService;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly ILogger<MoveToCategoryCommandHandler> _logger;

    public MoveToCategoryCommandHandler(IRestaurantAuthorizationService authorizationService, IRestaurantsRepository restaurantsRepository, ICategoriesRepository categoriesRepository, IDishesRepository dishesRepository, ILogger<MoveToCategoryCommandHandler> logger)
    {
        _authorizationService = authorizationService;
        _restaurantsRepository = restaurantsRepository;
        _categoriesRepository = categoriesRepository;
        _dishesRepository = dishesRepository;
        _logger = logger;
    }

    public async Task Handle(MoveToCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                   ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to access this restaurant.");

            if(!await _categoriesRepository.Exists(request.CategoryId))
                throw new ResourseNotFoundException(nameof(Category), request.CategoryId.ToString());

            _logger.LogInformation("Moving dish {DishId} to category {CategoryId} in restaurant {RestaurantId}.", request.DishId, request.CategoryId, request.RestaurantId);

            var dish = await _dishesRepository.GetByIdAsync(request.DishId)
                ?? throw new ResourseNotFoundException(nameof(Dish), request.DishId.ToString());

            dish.CategoryId = request.CategoryId;
            await _restaurantsRepository.CommitAsync();

            _logger.LogInformation("Successfully moved dish {DishId} to category {CategoryId} in restaurant {RestaurantId}.", request.DishId, request.CategoryId, request.RestaurantId);
        }
        catch (ResourseNotFoundException ex)
        {
            throw;
        }
        catch (UnAuthorizedException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while moving dish {DishId} to category {CategoryId} in restaurant {RestaurantId}.", request.DishId, request.CategoryId, request.RestaurantId);
            throw;
        }
    }
}
