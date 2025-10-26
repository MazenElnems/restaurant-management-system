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
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

    public GetCategoryByIdQueryHandler(IMapper mapper, IRestaurantsRepository restaurantsRepository, ILogger<GetCategoryByIdQueryHandler> logger)
    {
        _mapper = mapper;
        _restaurantsRepository = restaurantsRepository;
        _logger = logger;
    }

    public async Task<GetCategoryByIdDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await Task.CompletedTask;
            return new GetCategoryByIdDto();
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
