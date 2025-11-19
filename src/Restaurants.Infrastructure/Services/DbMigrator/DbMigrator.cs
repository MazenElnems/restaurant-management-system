using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Infrastructure.Data;
using Restaurants.Infrastructure.Services.DbMigrator.Interfaces;

namespace Restaurants.Infrastructure.Services.DbMigrator;

internal class DbMigrator : IDbMigrator
{
    private readonly RestaurantDbContext _db;
    private readonly ILogger<DbMigrator> _logger;

    public DbMigrator(RestaurantDbContext db, ILogger<DbMigrator> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void Migrate()
    {
        try
        {
            if (_db.Database.CanConnect() && _db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("An error occurred while migrating the database: {Message}", ex.Message);
            throw;
        }
    }
}
