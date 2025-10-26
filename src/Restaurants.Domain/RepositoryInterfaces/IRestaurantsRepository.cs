using Restaurants.Domain.Entities;

namespace Restaurants.Domain.RepositoryInterfaces;

public interface IRestaurantsRepository
{
    Task<List<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> AddAsync(Restaurant entity);
    Task<int> CommitAsync();
    Task<int> DeleteAsync(Restaurant entity);
    Task<bool> Exists(int id);
}
