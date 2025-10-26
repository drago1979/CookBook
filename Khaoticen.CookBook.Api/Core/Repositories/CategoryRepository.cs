using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Api.RequestDtos;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class CategoryRepository(AppDbContext db) : BaseRepository<Category>(db)
{
    public async Task<Category?> GetByNameAsync(string name) 
        => await Table.FirstOrDefaultAsync(c => c.Name == name);

    public async Task<Category?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await Table
            .Include(c => c.Recipes)
            .ThenInclude(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<List<Category>> GetAllByIdsAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.Distinct().ToList();

        if (idList.Count == 0)
            return [];

        return await Table
            .Where(c => idList.Contains(c.Id))
            .ToListAsync();
    }

    public async Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
            return [];

        return await Table
            .Where(c => idList.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();
    }
    
 // todo!!! : cleaner ?
    // public async Task<(List<Category> Items, int TotalCount)> GetAllPaginatedAsync(CategoriesAllRequest request)
    // {
    public async Task<(List<Category> Items, int TotalCount)> GetAllPaginatedAsync(
        int page = QueryConstants.InitPageNumber,
        int pageSize = QueryConstants.MaxPageSize,
        string sortBy = QueryConstants.DefaultSortBy,
        SortDirection sortDirection = SortDirection.Asc,
        string? searchColumn = null,
        string? searchValue = null
        )
    {
        var query = Table.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchColumn) && !string.IsNullOrWhiteSpace(searchValue))
            query = ApplyFiltering(query, searchColumn, searchValue);


        query = ApplySorting(query, sortBy, sortDirection);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    
    private IQueryable<Category> ApplyFiltering(
        IQueryable<Category> query,
        string searchColumn,
        string searchValue)
    {
        if (!CategoryQueryMetadata.Filters.TryGetValue(searchColumn, out var propertyExpr))
            throw new ArgumentException($"Unknown search column '{searchColumn}'");

        var parameter = propertyExpr.Parameters[0]; // Category c
        var propertyAccess = propertyExpr.Body;    // c.Name or c.Description

        // EF.Functions.Like(c.Prop, "%value%")
        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) })!;

        var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
        var pattern = Expression.Constant($"%{searchValue.Trim()}%");

        var likeCall = Expression.Call(likeMethod, efFunctions, propertyAccess, pattern);

        var lambda = Expression.Lambda<Func<Category, bool>>(likeCall, parameter);

        return query.Where(lambda);
    }

    
    private IQueryable<Category> ApplySorting(IQueryable<Category> query, string sortBy, SortDirection sortDirection)
    {
        var sortExpr = CategoryQueryMetadata.Sorts[sortBy];

        return sortDirection == SortDirection.Asc
            ? query.OrderBy(sortExpr)
            : query.OrderByDescending(sortExpr);
    }   
}