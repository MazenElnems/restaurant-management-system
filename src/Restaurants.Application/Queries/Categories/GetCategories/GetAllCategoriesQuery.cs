using MediatR;
using Restaurants.Application.DTOs.Categories;

namespace Restaurants.Application.Queries.Categories.GetCategories;

public class GetAllCategoriesQuery(int restaurantId) : IRequest<List<GetAllCategoriesDto>> 
{
    public int RestaurantId { get; init; } = restaurantId;
}
