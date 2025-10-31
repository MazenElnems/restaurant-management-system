using Restaurants.Domain.Entities;
using Restaurants.Domain.Enums;

namespace Restaurants.Domain.Interfaces;

public interface IRestaurantAuthorizationService
{
    bool Authorize(Restaurant restaurant, RestaurantOperation operation);
}
