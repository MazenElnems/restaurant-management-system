using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Categories.GetCategories;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdDto>
{
    private readonly IMapper _mapper;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public GetCategoryByIdQueryHandler(IMapper mapper, ICategoriesRepository categoriesRepository, ILogger<GetCategoryByIdQueryHandler> logger, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService authorizationService)
    {
        _mapper = mapper;
        _categoriesRepository = categoriesRepository;
        _logger = logger;
        _restaurantsRepository = restaurantsRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetCategoryByIdDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Domain.Entities.Restaurant), request.RestaurantId.ToString());

            if (!_authorizationService.Authorize(restaurant, RestaurantOperation.Read))
                throw new UnAuthorizedException("You are not authorized to access this restaurant's data.");

            _logger.LogInformation("Getting category by id {CategoryId} for restaurant {RestaurantId}", request.Id, request.RestaurantId);

            var category = await _categoriesRepository.GetByIdWithDishesAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Category), request.Id.ToString());

            var dto = _mapper.Map<GetCategoryByIdDto>(category); 
            return dto;
        }
        catch(UnAuthorizedException ex)
        {
            throw;
        }
        catch (ResourseNotFoundException ex)
        {
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting category by id {CategoryId}", request.Id);
            throw;
        }
    }
}
