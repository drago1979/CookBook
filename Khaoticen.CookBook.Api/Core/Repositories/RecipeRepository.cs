using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class RecipeRepository(AppDbContext db)
{
    public async Task<Recipe?> GetByIdWithReviewsAsync(Guid id)
    {
        return await db.Recipes
            .Include(r => r.Reviews)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    
}