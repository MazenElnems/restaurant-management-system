using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces;

public interface IDishesRepository
{
    Task<List<Dish>> GetAllByRestaurantIdAsync(int restaurantId);
    Task<Dish?> GetByRestaurantIdAsync(int id,int restaurantId);
    Task<Dish?> GetByIdAsync(int id);
    Task<List<Dish>> GetByCategoryIdAsync(int categoryId);
    Task<int> CommitAsync();
    Task<int> DeleteAsync(Dish dish);
}
