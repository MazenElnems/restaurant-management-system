using MediatR;
using Restaurants.Application.DTOs.Restaurants;

namespace Restaurants.Application.Queries.Restaurants.GetRestaurantQueries;

public class GetAllRestaurantsQuery : IRequest<List<GetAllRestaurantsDto>>
{

}
