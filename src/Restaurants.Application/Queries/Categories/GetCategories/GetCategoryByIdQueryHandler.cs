using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.CustomExceptions;
using Restaurants.Application.DTOs.Categories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;

namespace Restaurants.Application.Queries.Categories.GetCategories;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdDto>
{
    private readonly IMapper _mapper;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

    public GetCategoryByIdQueryHandler(IMapper mapper, ICategoriesRepository categoriesRepository, ILogger<GetCategoryByIdQueryHandler> logger, IRestaurantsRepository restaurantsRepository)
    {
        _mapper = mapper;
        _categoriesRepository = categoriesRepository;
        _logger = logger;
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task<GetCategoryByIdDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!await _restaurantsRepository.Exists(request.RestaurantId))
                throw new ResourseNotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var category = await _categoriesRepository.GetByIdWithDishesAsync(request.Id)
                ?? throw new ResourseNotFoundException(nameof(Category), request.Id.ToString());

            var dto = _mapper.Map<GetCategoryByIdDto>(category); 
            return dto;
        }
        catch(ResourseNotFoundException ex)
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
