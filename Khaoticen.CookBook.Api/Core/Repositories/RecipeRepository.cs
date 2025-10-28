using System.Reflection;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query;
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

    public async Task<(List<Recipe> Items, int TotalCount)> GetAllPaginatedAsync(RecipesAllRequest request)
    {
        IQueryable<Recipe>? query = null;

        if (request.CategoryId is { } categoryId)
        {
            query = GetCategoryIdFilteringQuery(categoryId);
        }

        var (page, pageSize, sortBy, sortDirection, searchColumn, searchValue) =
            GetPaginationAndFilteringParams(request);

        var (items, totalCount) =
            await GetAllPaginatedAsync(page, pageSize, sortBy, sortDirection, searchColumn, searchValue, query);

        return (items, totalCount);
    }

    public async Task<(List<Recipe> Items, int TotalCount)> GetAllWithDeletedPaginatedAsync(RecipesAllRequest request)
    {
        var query = request.CategoryId is { } categoryId
            ? GetCategoryIdFilteringQuery(categoryId)
            : Table.AsQueryable();

        query = query.IgnoreQueryFilters();

        var (page, pageSize, sortBy, sortDirection, searchColumn, searchValue) =
            GetPaginationAndFilteringParams(request);

        var (items, totalCount) =
            await GetAllWithDeletedPaginatedAsync(page, pageSize, sortBy, sortDirection, searchColumn, searchValue,
                query);

        return (items, totalCount);
    }

    protected IQueryable<Recipe> GetCategoryIdFilteringQuery(Guid? categoryId) // todo!!! moved to base
    {
        var query = Table.AsQueryable();

        return categoryId != null ? query.Where(r => r.Categories.Any(c => c.Id == categoryId)) : query;
    }

    protected static string? TryGetOptionalStringProperty(object request, string propName) // todo!!! moved to base
    {
        if (request == null) return null;

        var pi = request.GetType()
            .GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        return pi == null ? null : pi.GetValue(request) as string;
    }


    protected (int page, int pageSize, string sortBy, SortDirection sortDirection, string? searchColumn, string
        ? // todo!!! moved to base
        searchValue) GetPaginationAndFilteringParams<TRequest>(TRequest request)
        where TRequest : BasePaginatedSortedRequest
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var (page, pageSize, sortBy, sortDirection) = GetPaginationParams(request);

        // Filtering
        var searchColumn = TryGetOptionalStringProperty(request, nameof(CategoriesAllRequest.SearchColumn));
        var searchValue = TryGetOptionalStringProperty(request, nameof(CategoriesAllRequest.SearchValue));

        if (string.IsNullOrWhiteSpace(searchColumn) || string.IsNullOrWhiteSpace(searchValue))
        {
            searchColumn = null;
            searchValue = null;
        }

        return (page, pageSize, sortBy, sortDirection, searchColumn, searchValue);
    }
}