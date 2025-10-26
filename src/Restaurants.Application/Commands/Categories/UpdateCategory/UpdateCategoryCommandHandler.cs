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
    private readonly IRestaurantsRepository  _restaurantsRepository;
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

    public UpdateCategoryCommandHandler(IRestaurantsRepository  restaurantsRepository,ILogger<GetAllCategoriesQueryHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
    }   

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {

            await Task.CompletedTask;
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
