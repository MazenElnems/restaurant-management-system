using Restaurants.Domain.Entities;
using System.Linq.Expressions;

namespace Restaurants.Domain.RepositoryInterfaces;

public interface IRestaurantsRepository
{
    Task<List<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<Restaurant?> GetByIdAsync<T>(int id, Expression<Func<Restaurant,T>> includeSelector);
    Task<Restaurant?> GetRestaurantWithCategoriesAsync(int id);
    Task<Restaurant?> GetRestaurantWithCategoriesAsync(int id, Expression<Func<Category, bool>> categoryFilter,bool includeDishes = false);
    Task<int> AddAsync(Restaurant entity);
    Task<int> CommitAsync();
    Task<int> DeleteAsync(Restaurant entity);
    Task<bool> ExistsAsync(int id);
}
