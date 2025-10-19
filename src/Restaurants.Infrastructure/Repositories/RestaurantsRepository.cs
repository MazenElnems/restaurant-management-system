using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.RepositoryInterfaces;
using Restaurants.Infrastructure.Data;

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
        await _db.SaveChangesAsync(); 
        return entity.Id;
    }

    public async Task<List<Restaurant>> GetAllAsync()
    {
        return await _db.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await _db.Restaurants
            .Include(r => r.Categories)
            .ThenInclude(c => c.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
