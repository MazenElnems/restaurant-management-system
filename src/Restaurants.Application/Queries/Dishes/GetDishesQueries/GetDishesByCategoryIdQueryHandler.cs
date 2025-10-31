using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Commands.Dishes.MoveDishesCommands;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Dishes;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Dishes.GetDishesQueries;

public class GetDishesByCategoryIdQueryHandler : IRequestHandler<GetDishesByCategoryIdQuery, List<GetAllDishesDto>>
{
    private readonly IRestaurantAuthorizationService _authorizationService;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly ILogger<GetDishesByCategoryIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetDishesByCategoryIdQueryHandler(IRestaurantAuthorizationService authorizationService, IRestaurantsRepository restaurantsRepository, ICategoriesRepository categoriesRepository, IDishesRepository dishesRepository, ILogger<GetDishesByCategoryIdQueryHandler> logger, IMapper mapper)
    {
        _authorizationService = authorizationService;
        _restaurantsRepository = restaurantsRepository;
        _categoriesRepository = categoriesRepository;
        _dishesRepository = dishesRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<GetAllDishesDto>> Handle(GetDishesByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                   ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!_authorizationService.Authorize(restaurant, RestaurantOperation.Read))
                throw new UnAuthorizedException("You are not authorized to access this restaurant.");

            if (!await _categoriesRepository.Exists(request.CategoryId))
                throw new ResourseNotFoundException(nameof(Category), request.CategoryId.ToString());

            _logger.LogInformation("Fetching dishes for category {CategoryId} in restaurant {RestaurantId}.", request.CategoryId, request.RestaurantId);

            var dishes = await _dishesRepository.GetByCategoryIdAsync(request.CategoryId);
            var dto = _mapper.Map<List<GetAllDishesDto>>(dishes);
            return dto;
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
            _logger.LogError(ex, "An error occurred while fetching dishes for category {CategoryId} in restaurant {RestaurantId}.", request.CategoryId, request.RestaurantId);
            throw;
        }
    }
}
