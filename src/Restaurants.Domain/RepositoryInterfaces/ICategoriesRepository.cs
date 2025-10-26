using Restaurants.Domain.Entities;
using System.Linq.Expressions;

namespace Restaurants.Domain.RepositoryInterfaces;

public interface ICategoriesRepository
{
    Task<List<Category>> GetByRestaurantIdAsync(int restaurantId);
    Task<int> CommitAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetByIdWithIncludsAsync<T>(int id, Expression<Func<Category,T>> expression);
    Task<int> DeleteAsync(Category category);
}
