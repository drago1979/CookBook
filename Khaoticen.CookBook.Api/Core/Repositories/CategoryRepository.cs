using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class CategoryRepository (AppDbContext db): BaseRepository<Category>(db)
{
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await db.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<Category?> GetByIdWithRecipesAndCategoriesAsync(Guid id) =>
        await db.Categories
            .Include(c => c.Recipes)
            .ThenInclude(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);
    
    public async Task<List<Category>> GetAllByIdsAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.Distinct().ToList();

        if (idList.Count == 0)
            return [];

        return await db.Categories
            .Where(c => idList.Contains(c.Id))
            .ToListAsync();
    }
    
    public async Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids)
    {
        // todo!!: check the method 
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
            return [];

        return await db.Categories
            .Where(c => idList.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();
    }
    
}