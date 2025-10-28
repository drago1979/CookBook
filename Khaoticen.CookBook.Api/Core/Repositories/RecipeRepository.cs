using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class RecipeRepository(AppDbContext db, IRecipeQueryMetadata metadata) : BaseRepository<Recipe>(db, metadata)
{
    public async Task<Recipe?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await Table
            .Include(r => r.Reviews)
            .Include(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<(List<Recipe> Items, int TotalCount)> GetAllPaginatedAsync(RecipesAllRequest request,
        bool withDeleted = false)
    {
        var query = Table.AsQueryable();

        // Relationships
        query = ApplyRelationalFiltering(query, request.CategoryId);

        // Deleted-or-not
        if (withDeleted) query = query.IgnoreQueryFilters();

        var (items, totalCount) =
            await base.GetAllPaginatedAsync<RecipesAllRequest>(request, query);

        return (items, totalCount);
    }


    protected IQueryable<Recipe> ApplyRelationalFiltering(IQueryable<Recipe> query, Guid? categoryId)
    {
        if (categoryId.HasValue)
        {
            query = query.Where(r => r.Categories.Any(c => c.Id == categoryId));
        }

        return query;
    }
}