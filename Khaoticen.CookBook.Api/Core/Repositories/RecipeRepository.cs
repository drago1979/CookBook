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

        // Relationships
        query = ApplyRelationalFiltering(query, request.CategoryId);

        // Deleted-or-not
        if (withDeleted) query = query.IgnoreQueryFilters();

        var (items, totalCount) =
            await base.GetAllPaginatedAsync<RecipesAllRequest>(request, query);

        return (items, totalCount);
    }

    /// <summary>
    /// Filters the provided query to apply relational filtering based on the specified category ID.
    /// </summary>
    /// <param name="query">
    /// The queryable object of type <see cref="Recipe"/> that represents the base dataset to filter.
    /// </param>
    /// <param name="categoryId">
    /// The optional category ID to filter recipes by. If specified, only recipes associated with the given category will be included.
    /// </param>
    /// <return>
    /// The filtered queryable object containing recipes, potentially narrowed down to those related to the specified category.
    /// </return>
    protected IQueryable<Recipe> ApplyRelationalFiltering(IQueryable<Recipe> query, Guid? categoryId)
    {
        if (categoryId.HasValue)
        {
            query = query.Where(r => r.Categories.Any(c => c.Id == categoryId));
        }

        return query;
    }
}