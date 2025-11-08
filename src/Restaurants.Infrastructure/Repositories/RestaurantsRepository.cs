using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
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

    public async Task<bool> Exists(int id)
    {
        return await _db.Restaurants.AnyAsync(r => r.Id == id);
    }

    public async Task<(IEnumerable<Restaurant>,int)> GetPagedAsync(string? searchString, string sortBy, int pageNumber, int pageSize, bool ascending = false)
    {
        IQueryable<Restaurant> query = _db.Restaurants;

        // Filter
        if (!string.IsNullOrEmpty(searchString))
        {
            var searchStringLower = searchString.ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(searchStringLower)
                                || (r.Description != null && r.Description.ToLower().Contains(searchStringLower)));
        }

        int rowsCount = await query.CountAsync();

        // Sorting
        query = sortBy switch
        {
            RestaurantSortByOptions.Name => ascending ? query.OrderBy(r => r.Name) : query.OrderByDescending(r => r.Name),
            RestaurantSortByOptions.CreatedBy => ascending ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt),
            _ => query.OrderByDescending(r => r.CreatedAt)
        };

        // Pagination
        var result = await query.Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return (result, rowsCount);
    }

    public async Task<int> GetNumberOfOwnedRestaurants(int ownerId)
    {
        return await _db.Restaurants.CountAsync(r => r.OwnerId == ownerId);
    }
}
