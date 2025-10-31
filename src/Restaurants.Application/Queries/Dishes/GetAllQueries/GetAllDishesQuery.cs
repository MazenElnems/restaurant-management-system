using MediatR;
using Restaurants.Application.DTOs.Dishes;
using System.Text.Json.Serialization;

namespace Restaurants.Application.Queries.Dishes.GetDishesQueries;

public class GetAllDishesQuery(int restaurantId) : IRequest<List<GetAllDishesDto>>
{
    [JsonIgnore]
    public int RestaurantId { get; } = restaurantId;
}
