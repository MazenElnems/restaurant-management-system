using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Application.Queries.Categories.GetCategories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoriesRepository  _categoriesRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
    private readonly IRestaurantsRepository _restaurantsRepository;

    public UpdateCategoryCommandHandler(ICategoriesRepository categoriesRepository, ILogger<GetAllCategoriesQueryHandler> logger, IRestaurantsRepository restaurantsRepository)
    {
        _categoriesRepository = categoriesRepository;
        _logger = logger;
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _restaurantsRepository.Exists(request.RestaurantId))
                throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var category = await _categoriesRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Category), request.Id.ToString()); ;

            category.Name = request.Name;
            category.Description = request.Description;

            await _categoriesRepository.CommitAsync();
        }
        catch(ResourseNotFoundException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating category with id {CategoryId}", request.Id);
            throw;
        }
    }
}
