using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Constants;

public static class RestaurantSortByOptions
{
    public const string Name = nameof(Restaurant.Name);
    public const string CreatedBy = nameof(Restaurant.CreatedAt);
}
