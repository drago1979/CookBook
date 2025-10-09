using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class RecipeRepository(AppDbContext db)
{
    public async Task<Recipe?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await db.Recipes
            .Include(r => r.Reviews)
            .Include(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);


    public async Task<List<Guid>> GetOwnCategoryIdsAsync(Recipe recipe) =>
        await db.Categories
            .Where(c => c.Recipes.Any(r => r.Id == recipe.Id))
            .Select(c => c.Id)
            .ToListAsync();
}