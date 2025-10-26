using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;
using Restaurants.Infrastructure.Data;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class CategoriesRepository : ICategoriesRepository
{
    private readonly RestaurantDbContext _db;

    public CategoriesRepository(RestaurantDbContext db)
    {
        _db = db;
    }

    public async Task<int> CommitAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Category category)
    {
        _db.Categories.Remove(category);
        return await CommitAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByIdWithIncludsAsync<T>(int id, Expression<Func<Category, T>> expression)
    {
        return await _db.Categories
            .Include(expression)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Category>> GetByRestaurantIdAsync(int restaurantId)
    {
        return await _db.Categories.Where(c => c.RestaurantId == restaurantId)
            .ToListAsync();
    }
}
