using MediatR;
using Restaurants.Application.DTOs.Restaurants;

namespace Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

public class GetRestaurantByIdQuery(int id) : IRequest<RestaurantDto?>
{
    public int Id { get; init; } = id;
}
