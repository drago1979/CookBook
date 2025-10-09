using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Infrastructure.Db;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // todo: note-po konkretnom tipu
        // modelBuilder.Base<Recipe>().HasQueryFilter(r => !r.IsDeleted);


        // modelBuilder.Base<ISoftDeletable>().HasQueryFilter(r => r.DeletedAt == null);
        // modelBuilder.Base<BaseSoftDeletableEntity>().HasQueryFilter(r => r.DeletedAt == null);
        // todo: note-po interfejsu

        base.OnModelCreating(modelBuilder);
        
        // Data seeding
        modelBuilder.Entity<Category>().HasData(
            new Category {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Default" 
            },
            new Category
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Sweets"
            }
        );
        
        // Apply global filter to all BaseSoftDeletableEntity descendants
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType)) continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var prop = Expression.Property(parameter, nameof(BaseSoftDeletableEntity.DeletedAt));
            var condition = Expression.Equal(prop, Expression.Constant(null, typeof(DateTime?)));
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}