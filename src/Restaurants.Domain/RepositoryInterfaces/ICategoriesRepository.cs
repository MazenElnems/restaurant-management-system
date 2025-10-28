using Restaurants.Domain.Entities;

namespace Restaurants.Domain.RepositoryInterfaces;

public interface ICategoriesRepository
{
    Task<int> CommitAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<int> DeleteAsync(Category category);
    Task<Category?> GetByIdWithDishesAsync(int id);
    Task<List<Category>> GetByRestaurantId(int restaurantId);
}
