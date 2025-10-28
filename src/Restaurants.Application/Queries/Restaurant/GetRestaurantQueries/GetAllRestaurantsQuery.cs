using MediatR;
using Restaurants.Application.DTOs.Restaurants;

namespace Restaurants.Application.Queries.Restaurant.GetRestaurantQueries;

public class GetAllRestaurantsQuery : IRequest<List<GetAllRestaurantsDto>>
{

}
