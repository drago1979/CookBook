using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class RecipeRepository(AppDbContext db, IRecipeQueryMetadata metadata) : BaseRepository<Recipe>(db, metadata)
{
    /// <summary>
    /// Retrieves a recipe by its unique identifier with all related data included, such as reviews and categories.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <param name="withDeleted">
    /// A boolean value indicating whether to include soft-deleted recipes in the search.
    /// If true, soft-deleted recipes will also be considered.
    /// </param>
    /// <returns>
    /// The recipe matching the given identifier, including its related data, if found; otherwise, null.
    /// </returns>
    public async Task<Recipe?> GetByIdIncludeAllRelatedAsync(Guid id, bool withDeleted = false)
    {
        var query = Table.AsQueryable();

        if (withDeleted) query = query.IgnoreQueryFilters();
        
        return await query
            .Include(r => r.Reviews)
            .Include(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);
    }


    /// <summary>
    /// Retrieves a paginated list of Recipe entities based on the provided request parameters.
    /// </summary>
    /// <param name="request">
    /// The request object containing pagination, sorting, and filtering criteria for fetching the recipes.
    /// </param>
    /// <param name="withDeleted">
    /// A boolean flag that specifies whether to include soft-deleted recipes in the result.
    /// </param>
    /// <returns>
    /// A tuple containing a list of Recipe entities and the total count of records matching the criteria.
    /// </returns>
    public async Task<(List<Recipe> Items, int TotalCount)> GetAllPaginatedAsync(RecipesAllRequest request,
        bool withDeleted = false)
    {
        var query = Table.AsQueryable();
        
        query = ApplyRelationalFiltering(request, query);
        
        var (items, totalCount) = await base.GetAllPaginatedAsync(request, query);

        return (items, totalCount);
    }

    /// <summary>
    /// Applies filtering to the given query based on the relational properties specified in the request.
    /// </summary>
    /// <param name="request">
    /// The request object containing filtering criteria, such as the category identifier.
    /// </param>
    /// <param name="query">
    /// The initial query to which the filtering is applied. If null, a new query is instantiated using the base Table.
    /// </param>
    /// <returns>
    /// The query after applying the filtering based on the relational properties.
    /// </returns>
    private IQueryable<Recipe> ApplyRelationalFiltering(RecipesAllRequest request, IQueryable<Recipe>? query = null)
    {
        query ??= Table.AsQueryable();
        
        if (request.CategoryId is { } categoryId)
        {
            query = query.Where(r => r.Categories.Any(c => c.Id == categoryId));
        }

        return query;
    }
}