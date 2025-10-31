using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Data;

public class RestaurantDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Category> Categories { get; set; }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .UseTphMappingStrategy()
            .HasDiscriminator<string>("UserType")
            .HasValue<Admin>("Admin")
            .HasValue<Owner>("Owner")
            .HasValue<Staff>("Staff");

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.ToTable("Restaurants");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                  .HasColumnType("VARCHAR(200)");

            entity.Property(r => r.Description)
                  .HasColumnType("VARCHAR(MAX)");

            entity.Property(r => r.ContactEmail)
                  .HasColumnType("VARCHAR(200)");

            entity.Property(r => r.ContactNumber)
                  .HasColumnType("VARCHAR(11)");

            entity.OwnsOne(r => r.Address, address =>
            {
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(100);
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(200);
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").HasMaxLength(15);
            });

            entity.HasOne(r => r.Owner)
                  .WithMany(o => o.Restaurants)
                  .HasForeignKey(r => r.OwnerId)
                  .IsRequired();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .HasColumnType("VARCHAR(200)");

            entity.Property(c => c.Description)
                  .HasColumnType("VARCHAR(MAX)");

            entity.HasOne(c => c.Restaurant)
                  .WithMany(r => r.Categories)
                  .HasForeignKey(c => c.RestaurantId)
                  .IsRequired();
        });

        modelBuilder.Entity<Dish>(entity =>
        {
            entity.ToTable("Dishes");

            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .HasColumnType("VARCHAR(200)");

            entity.Property(c => c.Description)
                  .HasColumnType("VARCHAR(MAX)");

            entity.HasOne(c => c.Category)
                  .WithMany(r => r.Dishes)
                  .HasForeignKey(c => c.CategoryId)
                  .IsRequired();
        });
    }
}
