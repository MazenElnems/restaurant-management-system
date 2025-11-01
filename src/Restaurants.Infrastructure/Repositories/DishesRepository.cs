using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Data;

namespace Restaurants.Infrastructure.Repositories;

public class DishesRepository : IDishesRepository
{
    private readonly RestaurantDbContext _db;

    public DishesRepository(RestaurantDbContext db)
    {
        _db = db;
    }

    public async Task<List<Dish>> GetAllByRestaurantIdAsync(int restaurantId)
    {
        return await _db.Restaurants
            .Where(r => r.Id == restaurantId)
            .SelectMany(r => r.Categories)
            .SelectMany(c => c.Dishes)
            .ToListAsync();
    }

    public Task<List<Dish>> GetByCategoryIdAsync(int categoryId)
    {
        return _db.Dishes
            .Where(d => d.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Dish?> GetByIdAsync(int id)
    {
        return await _db.Dishes.FindAsync(id);
    }

    public async Task<int> CommitAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(Dish dish)
    {
        _db.Dishes.Remove(dish);
        return await CommitAsync();
    }

    public async Task<Dish?> GetByRestaurantIdAsync(int id, int restaurantId)
    {
        return await _db.Restaurants
            .Where(r => r.Id == restaurantId)
            .SelectMany(r => r.Categories)
            .SelectMany(c => c.Dishes)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
}