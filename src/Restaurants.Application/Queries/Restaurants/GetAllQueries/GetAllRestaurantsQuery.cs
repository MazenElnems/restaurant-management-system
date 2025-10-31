using MediatR;
using Restaurants.Application.DTOs.Restaurants;

namespace Restaurants.Application.Queries.Restaurants.GetAllQueries;

public class GetAllRestaurantsQuery : IRequest<List<GetAllRestaurantsDto>>
{

}
