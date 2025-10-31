using MediatR;
using Restaurants.Application.DTOs.Restaurants;

namespace Restaurants.Application.Queries.Restaurants.GetRestaurantQueries;

public class GetRestaurantByIdQuery(int id) : IRequest<GetRestaurantByIdDto>
{
    public int Id { get; init; } = id;
}
