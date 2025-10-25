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
}
