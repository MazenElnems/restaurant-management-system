using MediatR;
using Restaurants.Application.DTOs.Categories;

namespace Restaurants.Application.Queries.Categories.GetCategories;

public class GetCategoryByIdQuery(int id, int restaurantId) : IRequest<GetCategoryByIdDto>
{
    public int Id { get; init; } = id;
    public int RestaurantId { get; set; } = restaurantId;
}
