using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class CategoryRepository (AppDbContext db)
{
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await db.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }
}