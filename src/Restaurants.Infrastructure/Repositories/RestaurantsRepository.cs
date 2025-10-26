using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;
using Restaurants.Infrastructure.Data;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository : IRestaurantsRepository
{
    private readonly RestaurantDbContext _db;

    public RestaurantsRepository(RestaurantDbContext db)
    {
        _db = db;
    }

    public async Task<int> AddAsync(Restaurant entity)
    {
        _db.Add(entity);
        await CommitAsync();
        return entity.Id;
    }

    public async Task<List<Restaurant>> GetAllAsync()
    {
        return await _db.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await _db.Restaurants
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> CommitAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Restaurant entity)
    {
        _db.Remove(entity);
        return await CommitAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _db.Restaurants.AnyAsync(r => r.Id == id);
    }

    public async Task<Restaurant?> GetByIdAsync<T>(int id, Expression<Func<Restaurant, T>> includeSelector)
    {
        return await _db.Restaurants
            .Include(includeSelector)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Restaurant?> GetByIdAsync<T>(int id, Expression<Func<Restaurant, T>> includeSelector, Expression<Func<Restaurant, bool>> filter)
    {
        return await _db.Restaurants
            .Include(includeSelector)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Restaurant?> GetRestaurantWithCategoriesAsync(int id)
    {
        return await GetRestaurantWithCategoriesAsync(id, c => true);
    }

    public async Task<Restaurant?> GetRestaurantWithCategoriesAsync(int id, Expression<Func<Category, bool>> categoryFilter, bool includeDishes = false)
    {
        var query =  _db.Restaurants
            .Include(r => r.Categories.AsQueryable().Where(categoryFilter));

        if(includeDishes)
            query.ThenInclude(c => c.Dishes);

        return await query.FirstOrDefaultAsync(r => r.Id == id);
    }
}
