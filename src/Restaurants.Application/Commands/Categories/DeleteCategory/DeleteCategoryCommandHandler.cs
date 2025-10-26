using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Application.Queries.Categories.GetCategories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

    public DeleteCategoryCommandHandler(ICategoriesRepository categoriesRepository, ILogger<GetAllCategoriesQueryHandler> logger, IRestaurantsRepository restaurantsRepository)
    {
        _categoriesRepository = categoriesRepository;
        _logger = logger;
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {

            await Task.CompletedTask;
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
