using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Application.Queries.Categories.GetAllQueries;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Commands.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoriesRepository  _categoriesRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IRestaurantAuthorizationService _authorizationService;

    public UpdateCategoryCommandHandler(ICategoriesRepository categoriesRepository, ILogger<GetAllCategoriesQueryHandler> logger, IRestaurantsRepository restaurantsRepository, IRestaurantAuthorizationService authorizationService)
    {
        _categoriesRepository = categoriesRepository;
        _logger = logger;
        _restaurantsRepository = restaurantsRepository;
        _authorizationService = authorizationService;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString()); 

            if(!_authorizationService.Authorize(restaurant, RestaurantOperation.Update))
                throw new UnAuthorizedException("You are not authorized to update category of this restaurant.");

            var category = await _categoriesRepository.GetByIdAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if(category.RestaurantId != request.RestaurantId)
                throw new UnAuthorizedException("Category does not belong to this restaurant.");

            _logger.LogInformation("Updating category with id {CategoryId}", request.Id);

            category.Name = request.Name;
            category.Description = request.Description;

            await _categoriesRepository.CommitAsync();
            _logger.LogInformation("Category with id {CategoryId} updated successfully", request.Id);
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
            _logger.LogError(ex, "An error occurred while updating category with id {CategoryId}", request.Id);
            throw;
        }
    }
}
