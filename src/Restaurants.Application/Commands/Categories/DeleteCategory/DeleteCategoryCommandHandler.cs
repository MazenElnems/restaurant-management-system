using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Queries.Categories.GetAllQueries;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoriesRepository  _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public DeleteCategoryCommandHandler(ILogger<GetAllCategoriesQueryHandler> logger, ICategoriesRepository categoriesRepository, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService authorizationService)
    {
        _logger = logger;
        _categoriesRepository = categoriesRepository;
        _restaurantsRepository = restaurantsRepository;
        _authorizationService = authorizationService;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to delete category from this restaurant.");

            var category = await _categoriesRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(category.RestaurantId != request.RestaurantId)
                throw new UnAuthorizedException("Category does not belong to this restaurant.");

            _logger.LogInformation("Deleting Category with ID: {CategoryId}", category.Id);

            await _categoriesRepository.DeleteAsync(category);
            _logger.LogInformation("Category with ID: {CategoryId} deleted successfully.", category.Id);
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
            _logger.LogError(ex, "An error occurred while deleting category with Id {CategoryId}", request.Id);
            throw;
        }
    }
}
