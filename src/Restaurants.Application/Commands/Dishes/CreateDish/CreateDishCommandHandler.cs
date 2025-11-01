using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Dishes.CreateDish;

public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateDishCommandHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public CreateDishCommandHandler(IRestaurantsRepository restaurantsRepository, ICategoriesRepository categoriesRepository, IMapper mapper, ILogger<CreateDishCommandHandler> logger, IRestaurantAuthorizationService authorizationService)
    {
        _restaurantsRepository = restaurantsRepository;
        _categoriesRepository = categoriesRepository;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to add dish to this restaurant.");

            var category = await _categoriesRepository.GetByIdAsync(request.CategoryId)
                ?? throw new ResourseNotFoundException(nameof(Category), request.CategoryId.ToString());

            if(category.RestaurantId != request.RestaurantId)
                throw new UnAuthorizedException("Category does not belong to this restaurant.");

            _logger.LogInformation("Creating new dish for restaurant with id {RestaurantId}", request.RestaurantId);

            var dish = _mapper.Map<Dish>(request);

            category.Dishes.Add(dish);
            await _categoriesRepository.CommitAsync();

            _logger.LogInformation("New dish created successfully with ID: {DishId}", dish.Id);

            return dish.Id;
        }
        catch(UnAuthorizedException ex)
        {
            throw;
        }
        catch(ResourseNotFoundException ex)
        {
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding new dish to restaurant with id {RestaurantId}", request.RestaurantId);
            throw;
        }
    }
}

