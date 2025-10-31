using MediatR;
using Restaurants.Application.DTOs.Dishes;

namespace Restaurants.Application.Queries.Dishes.GetByCategoryIdQueries;

public class GetDishesByCategoryIdQuery(int restaurantId, int categoryId) : IRequest<List<GetAllDishesDto>>
{
    public int RestaurantId { get; } = restaurantId;
    public int CategoryId { get; } = categoryId;
}
