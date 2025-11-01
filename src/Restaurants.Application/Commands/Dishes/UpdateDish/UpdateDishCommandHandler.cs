using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Dishes.UpdateDish;

public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand>
{
    private readonly IDishesRepository _dishesRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<UpdateDishCommandHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public UpdateDishCommandHandler(IDishesRepository dishesRepository, ICategoriesRepository categoriesRepository, IRestaurantsRepository restaurantsRepository, ILogger<UpdateDishCommandHandler> logger, IRestaurantAuthorizationService authorizationService)
    {
        _dishesRepository = dishesRepository;
        _categoriesRepository = categoriesRepository;
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString()); 

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to update dish of this restaurant.");

            var dish = await _dishesRepository.GetByRestaurantIdAsync(request.Id, request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Dish), request.Id.ToString());

            _logger.LogInformation("Updating dish with id {DishId}", request.Id);

            dish.Name = request.Name;
            dish.Description = request.Description;
            dish.Price = request.Price;
            dish.KiloCalories = request.KiloCalories;
            dish.IsAvailable = request.IsAvailable;

            await _dishesRepository.CommitAsync();
            _logger.LogInformation("Dish with id {DishId} updated successfully", request.Id);
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
            _logger.LogError(ex, "An error occurred while updating dish with id {DishId}", request.Id);
            throw;
        }
    }
}

