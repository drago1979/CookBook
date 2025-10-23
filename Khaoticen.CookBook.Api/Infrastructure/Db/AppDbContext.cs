using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;
using Khaoticen.CookBook.Api.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Infrastructure.Db;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Global filter - soft deletable
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType)) continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var prop = Expression.Property(parameter, nameof(ISoftDeletable.DeletedAt));
            var condition = Expression.Equal(prop, Expression.Constant(null, typeof(DateTime?)));
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
        
        // Data seeding
        modelBuilder.Entity<Category>().HasData(
            new Category {
                Id = CategoryConstants.DefaultCategoryId,
                Name = CategoryConstants.DefaultCategoryName
            },
            new Category
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Sweets"
            }
        );
    }
}