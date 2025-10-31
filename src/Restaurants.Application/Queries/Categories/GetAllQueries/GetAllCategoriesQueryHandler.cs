using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Queries.Categories.GetAllQueries;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<GetAllCategoriesDto>>
{
    private readonly IMapper _mapper;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public GetAllCategoriesQueryHandler(ICategoriesRepository categoriesRepository, IMapper mapper, ILogger<GetAllCategoriesQueryHandler> logger, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService authorizationService)
    {
        _categoriesRepository = categoriesRepository;
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task<List<GetAllCategoriesDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Read))
                throw new UnAuthorizedException("You are not authorized to access this restaurant's categories.");

            _logger.LogInformation("Getting categories for RestaurantId: {RestaurantId}", request.RestaurantId);

            var categories = await _categoriesRepository.GetByRestaurantId(request.RestaurantId);

            var dto = _mapper.Map<List<GetAllCategoriesDto>>(categories);
            return dto;
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
            _logger.LogError(ex, "An error occurred while getting categories for RestaurantId: {RestaurantId}", request.RestaurantId);
            throw;
        }
    }
}
