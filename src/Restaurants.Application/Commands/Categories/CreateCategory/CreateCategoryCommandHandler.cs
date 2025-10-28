using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Commands.Categories.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(IRestaurantsRepository restaurantsRepository, IMapper mapper, ILogger<CreateCategoryCommandHandler> logger)
    {
        _restaurantsRepository = restaurantsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var category = _mapper.Map<Category>(request);

            restaurant.Categories.Add(category);
            await _restaurantsRepository.CommitAsync();

            _logger.LogInformation("New category created successfully {@Category}", category);

            return category.Id;
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
