using Khaoticen.CookBook.Api.Shared.Query;
using System.Linq.Expressions;
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
    /// Retrieves a paginated and sorted list of entities from the database,
    /// optionally including deleted entities, based on the specified request parameters.
    /// </summary>
    /// <param name="request">The request containing pagination, sorting, and filtering parameters.</param>
    /// <param name="query">An optional query to apply additional filters or operations. Null by default.</param>
    /// <param name="withDeleted">A flag indicating whether to include deleted entities in the result. False by default.</param>
    /// <typeparam name="TEntitiesAllRequest">The type of the request object used for pagination, sorting, and filtering.</typeparam>
    /// <returns>A tuple containing a list of entities and the total count of matching entities.</returns>
    protected async Task<(List<TEntity> Items, int TotalCount)>
        GetAllPaginatedAsync<TEntitiesAllRequest>(
            TEntitiesAllRequest request,
            IQueryable<TEntity>? query = null,
            bool withDeleted = false
        )
        where TEntitiesAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
    {
        query ??= Table.AsQueryable();
        
        // Deleted-or-not
        if (withDeleted) query = query.IgnoreQueryFilters();
        
        // Filtering
        query = ApplyFiltering(request, query);

        // Sorting         
        query = ApplySorting(request, query);

        var totalCount = await query.CountAsync();

        var page = request.Page;
        var pageSize = request.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    
    /// <summary>
    /// Updates an existing entity in the database context for tracking and saving.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
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
    
    private IQueryable<TEntity> ApplyFiltering<TEntitiesAllRequest>(TEntitiesAllRequest request,
        IQueryable<TEntity>? query = null)
        where TEntitiesAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
    {
        query ??= Table.AsQueryable();

        // Return if NO SEARCH values in REQUEST or in METADATA
        if (request.SearchColumn is not { } searchColumn || request.SearchValue is not { } searchValue ||
            metadata?.Filters is null)
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

    private IQueryable<TEntity> ApplySorting<TEntitiesAllRequest>(TEntitiesAllRequest request,
        IQueryable<TEntity>? query = null)
        where TEntitiesAllRequest : BasePaginatedSortedRequest
    {
        query ??= Table.AsQueryable();

        var sortBy = request.SortBy;
        var sortDirection = request.SortDirection;

        var sortExpr = metadata.Sorts[sortBy];

        return sortDirection == SortDirection.Asc
            ? query.OrderBy(sortExpr)
            : query.OrderByDescending(sortExpr);
    }
    #endregion
}