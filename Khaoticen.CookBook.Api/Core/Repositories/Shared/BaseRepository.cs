using Khaoticen.CookBook.Api.Shared.Query;
using System.Linq.Expressions;
using System.Reflection;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Interface;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories.Shared;

public abstract class
    BaseRepository<TEntity>(AppDbContext db, IEntityQueryMetadata<TEntity> metadata)
    where TEntity : BaseEntity
{
    protected DbSet<TEntity> Table => db.Set<TEntity>();

    #region CRUD

    /// <summary>
    /// Adds a new entity to the database context for tracking and saving.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    public void Add(TEntity entity)
    {
        Table.Add(entity);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to be retrieved.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Table.FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Retrieves paginated and optionally sorted results of entities, applying filtering and optional custom query modifications.
    /// </summary>
    /// <typeparam name="TEntitiesAllRequest">The request type containing pagination, sorting, and filtering details.</typeparam>
    /// <param name="request">The request object specifying pagination, sorting, and filtering parameters.</param>
    /// <param name="query">Optional custom query for further modifications. If not provided, the base query will be used.</param>
    /// <returns>A tuple containing a list of entities and the total count of matching entities.</returns>
    public async Task<(List<TEntity> Items, int TotalCount)> GetAllPaginatedAsync
        <TEntitiesAllRequest>
        (TEntitiesAllRequest request,
            IQueryable<TEntity>? query = null)
        where TEntitiesAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
    {
        query ??= Table.AsQueryable();

        // Filtering
        var (searchColumn, searchValue) = GetFilteringParams<TEntitiesAllRequest>(request);
        if (!string.IsNullOrWhiteSpace(searchColumn) && !string.IsNullOrWhiteSpace(searchValue))
            query = ApplyFiltering(query, searchColumn, searchValue);


        // Pagination & sorting
        var (page, pageSize, sortBy, sortDirection) = GetPaginationAndSortingParams(request);
        query = ApplySorting(query, sortBy, sortDirection);

        var (items, totalCount) =
            await GetAllPaginatedAsync(page, pageSize, query);

        return (items, totalCount);
    }

    public void Update(TEntity entity)
    {
        Table.Update(entity);
    }

    /// Removes a given entity from the database.
    /// This method marks the entity for removal from the corresponding database table,
    /// effectively scheduling it to be deleted upon the next save operation.
    /// It does not save changes to the database; to persist the deletion, a call to
    /// `SaveChangesAsync` on the database context must be made.
    /// Parameters:
    /// entity:
    /// The entity to be removed from the database.
    public void Delete(TEntity entity)
    {
        Table.Remove(entity);
    }

    #endregion

    #region SORTING - FILTERING
    
    private (string? searchColumn, string? searchValue) GetFilteringParams<TEntitiesAllRequest>(
        TEntitiesAllRequest request)
        where TEntitiesAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
    {
        ArgumentNullException.ThrowIfNull(request);

        var searchColumn = TryGetOptionalStringProperty(request, nameof(request.SearchColumn));
        var searchValue = TryGetOptionalStringProperty(request, nameof(request.SearchValue));

        if (string.IsNullOrWhiteSpace(searchColumn) || string.IsNullOrWhiteSpace(searchValue))
        {
            searchColumn = null;
            searchValue = null;
        }

        return (searchColumn, searchValue);
    }

    private static string?
        TryGetOptionalStringProperty<TRequest>(TRequest request, string propName)
    {
        if (request == null) return null;

        var pi = request.GetType()
            .GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        return pi == null ? null : pi.GetValue(request) as string;
    }

    private (int page, int pageSize, string sortBy, SortDirection sortDirection)
        GetPaginationAndSortingParams<TRequest>(
            TRequest request
        )
        where TRequest : BasePaginatedSortedRequest
    {
        // Pagination/sorting
        var page = request.Page;
        var pageSize = request.PageSize;
        var sortBy = request.SortBy;
        var sortDirection = request.SortDirection;

        return (page, pageSize, sortBy, sortDirection);
    }

    private async Task<(List<TEntity> Items, int TotalCount)>
        GetAllPaginatedAsync(
            int page = QueryConstants.InitPageNumber,
            int pageSize = QueryConstants.MaxPageSize,
            IQueryable<TEntity>? query = null
        )
    {
        query ??= Table.AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    private IQueryable<TEntity> ApplyFiltering(
        IQueryable<TEntity> query,
        string searchColumn,
        string searchValue)
    {
        if (metadata?.Filters is null)
            return query;
        
        if (!metadata.Filters.TryGetValue(searchColumn, out var propertyExpr))
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
    
    private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sortBy, SortDirection sortDirection)
    {
        var sortExpr = metadata.Sorts[sortBy];

        return sortDirection == SortDirection.Asc
            ? query.OrderBy(sortExpr)
            : query.OrderByDescending(sortExpr);
    }

    #endregion
}