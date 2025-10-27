using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class RecipeRepository(AppDbContext db, IRecipeQueryMetadata metadata) : BaseRepository<Recipe>(db, metadata)
{
    public async Task<List<Recipe>> GetAllWithDeletedAsync() =>
        await Table.IgnoreQueryFilters().ToListAsync();

    public async Task<Recipe?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await Table
            .Include(r => r.Reviews)
            .Include(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);
}