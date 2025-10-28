using Khaoticen.CookBook.Api.Shared.Query;
using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories.Shared;

public abstract class
    BaseRepository<TEntity>(
        AppDbContext db,
        IEntityQueryMetadata<TEntity> metadata) // todo: Prodji kroz ovo
    where TEntity : BaseEntity
    // where TBasePaginatedRequest : BasePaginatedSortedRequest
{
    protected readonly IEntityQueryMetadata<TEntity> Metadata = metadata;

    protected DbSet<TEntity> Table => db.Set<TEntity>();

    public void Add(TEntity entity)
    {
        Table.Add(entity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Table.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<TEntity>> GetAllAsync() // todo!!! remove
    {
        return await Table.ToListAsync();
    }

    protected (int page, int pageSize, string sortBy, SortDirection sortDirection) GetPaginationParams<TRequest>(
        TRequest request)
        where TRequest : BasePaginatedSortedRequest
    {
        // Pagination/sorting
        var page = request.Page;
        var pageSize = request.PageSize;
        var sortBy = request.SortBy;
        var sortDirection = request.SortDirection;

        return (page, pageSize, sortBy, sortDirection);
    }


    // todo!!! : cleaner ?
    // public async Task<(List<Category> Items, int TotalCount)> GetAllPaginatedAsync(CategoriesAllRequest request)
    // {
    public virtual async Task<(List<TEntity> Items, int TotalCount)> GetAllPaginatedAsync(
        int page = QueryConstants.InitPageNumber,
        int pageSize = QueryConstants.MaxPageSize,
        string sortBy = QueryConstants.DefaultSortBy,
        SortDirection sortDirection = SortDirection.Asc,
        string? searchColumn = null,
        string? searchValue = null,
        IQueryable<TEntity>? query = null
    )
    {
        query ??= Table.AsQueryable();

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


    public virtual async Task<(List<TEntity> Items, int TotalCount)> GetAllWithDeletedPaginatedAsync(
        int page = QueryConstants.InitPageNumber,
        int pageSize = QueryConstants.MaxPageSize,
        string sortBy = QueryConstants.DefaultSortBy,
        SortDirection sortDirection = SortDirection.Asc,
        string? searchColumn = null,
        string? searchValue = null,
        IQueryable<TEntity>? query = null
    )
    {
        query ??= Table.AsQueryable();

        query = query.IgnoreQueryFilters();

        var (items, totalCount) =
            await GetAllPaginatedAsync(page, pageSize, sortBy, sortDirection, searchColumn, searchValue, query);

        return (items, totalCount);
    }

    public void Update(TEntity entity)
    {
        Table.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Table.Remove(entity);
    }

    #region SORTING - FILTERING

    protected IQueryable<TEntity> ApplyFiltering(
        IQueryable<TEntity> query,
        string searchColumn,
        string searchValue)
    {
        if (!Metadata.Filters.TryGetValue(searchColumn, out var propertyExpr))
            throw new ArgumentException($"Unknown search column '{searchColumn}'");

        var parameter = propertyExpr.Parameters[0];
        var propertyAccess = propertyExpr.Body;

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) })!;

        var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
        var pattern = Expression.Constant($"%{searchValue.Trim()}%");

        var likeCall = Expression.Call(likeMethod, efFunctions, propertyAccess, pattern);

        var lambda = Expression.Lambda<Func<TEntity, bool>>(likeCall, parameter);

        return query.Where(lambda);
    }


    protected IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sortBy, SortDirection sortDirection)
    {
        var sortExpr = Metadata.Sorts[sortBy];

        return sortDirection == SortDirection.Asc
            ? query.OrderBy(sortExpr)
            : query.OrderByDescending(sortExpr);
    }

    #endregion
}