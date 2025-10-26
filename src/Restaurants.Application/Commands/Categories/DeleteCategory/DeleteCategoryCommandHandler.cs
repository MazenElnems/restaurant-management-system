using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.Queries.Categories.GetCategories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoriesRepository  _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

    public DeleteCategoryCommandHandler(ILogger<GetAllCategoriesQueryHandler> logger, ICategoriesRepository categoriesRepository, IRestaurantsRepository restaurantsRepository)
    {
        _logger = logger;
        _categoriesRepository = categoriesRepository;
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _restaurantsRepository.Exists(request.RestaurantId))
                throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var category = await _categoriesRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Category), request.Id.ToString());

            await _categoriesRepository.DeleteAsync(category);
            _logger.LogInformation("Category: {@Category} deleted successfully.", category);
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
