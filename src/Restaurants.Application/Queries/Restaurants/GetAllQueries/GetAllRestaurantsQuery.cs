using MediatR;
using Restaurants.Application.DTOs.Common;
using Restaurants.Application.DTOs.Restaurants;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Queries.Restaurants.GetAllQueries;

public class GetAllRestaurantsQuery : IRequest<PagedResult<GetAllRestaurantsDto>>
{
    public string? SearchString { get; set; }
    public string? SortBy { get; set; } = RestaurantSortByOptions.CreatedBy;
    public string? SortOrder { get; set; } = SortOrderOptions.Descending;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
