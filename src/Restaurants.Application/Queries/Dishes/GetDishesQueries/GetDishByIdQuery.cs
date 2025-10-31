using MediatR;
using Restaurants.Application.DTOs.Dishes;

namespace Restaurants.Application.Queries.Dishes.GetDishesQueries;

public class GetDishByIdQuery(int restaurantId, int id) : IRequest<GetDishDto>
{
    public int Id { get;  } = id;
    public int RestaurantId { get; } = restaurantId;
}
