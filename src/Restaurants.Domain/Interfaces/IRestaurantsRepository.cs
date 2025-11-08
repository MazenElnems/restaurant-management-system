using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces;

public interface IRestaurantsRepository
{
    Task<(IEnumerable<Restaurant>, int)> GetPagedAsync(string? searchString, string sortBy, int pageNumber, int pageSize , bool ascending = false);
    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> AddAsync(Restaurant entity);
    Task<int> CommitAsync();
    Task<int> DeleteAsync(Restaurant entity);
    Task<bool> Exists(int id);
    Task<int> GetNumberOfOwnedRestaurants(int ownerId);
}
