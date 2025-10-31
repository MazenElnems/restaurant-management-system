using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Categories.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public CreateCategoryCommandHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger, IRestaurantAuthorizationService authorizationService)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to add category to this restaurant.");

            _logger.LogInformation("Creating new category for restaurant with id {RestaurantId}", request.RestaurantId);

            var category = _mapper.Map<Category>(request);

            restaurant.Categories.Add(category);
            await _restaurantsRepository.CommitAsync();

            _logger.LogInformation("New category created successfully {@Category}", category);

            return category.Id;
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
            _logger.LogError(ex, "Error occurred while adding new category to restaurant with id {RestaurantId}", request.RestaurantId);
            throw;
        }
    }
}
