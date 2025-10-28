using System.Reflection;
using AutoMapper;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;

public abstract class BaseEntityService<
    TEntity,
    TEntityRepository,
    TEntityFactory,
    TCreateDto,
    TUpdateDto
>(
    AppDbContext db,
    IMapper mapper,
    TEntityRepository repository,
    TEntityFactory factory
)
    where TEntity : BaseEntity
    where TEntityRepository : BaseRepository<TEntity>
    where TEntityFactory : BaseFactory<TEntity, TCreateDto>
    where TCreateDto : ICreateDto
    where TUpdateDto : IUpdateDto
{
    protected readonly AppDbContext Db = db;
    protected readonly TEntityRepository Repository = repository;
    protected readonly IMapper Mapper = mapper;
    protected readonly TEntityFactory Factory = factory;

    #region CRUD

    public virtual async Task<List<TEntity>> GetAllAsync() // todo!! remove?
    {
        return await Repository.GetAllAsync();
    }

    private static string? TryGetOptionalStringProperty(object request, string propName) // todo!!! remove?
    {
        if (request == null) return null;

        var pi = request.GetType()
            .GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        return pi == null ? null : pi.GetValue(request) as string;
    }


    public (int page, int pageSize, string sortBy, SortDirection sortDirection, string? searchColumn, string?
        searchValue) GetPaginationParams<TRequest>(TRequest request) // todo!!! remove?
        where TRequest : BasePaginatedSortedRequest
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        // Page / PageSize / SortBy / SortDirection are available at compile-time on BasePaginatedSortedRequest
        int page = request.Page;
        int pageSize = request.PageSize;
        string sortBy = request.SortBy ?? QueryConstants.DefaultSortBy;
        SortDirection sortDirection = request.SortDirection;

        // SearchColumn / SearchValue may or may not exist in derived requests.
        // Try to read them via reflection in a safe manner (case-insensitive).
        var searchColumn = TryGetOptionalStringProperty(request, nameof(CategoriesAllRequest.SearchColumn));
        var searchValue = TryGetOptionalStringProperty(request, nameof(CategoriesAllRequest.SearchValue));

        // If either is missing/empty, we skip filtering.
        if (string.IsNullOrWhiteSpace(searchColumn) || string.IsNullOrWhiteSpace(searchValue))
        {
            searchColumn = null;
            searchValue = null;
        }
        
        return (page, pageSize, sortBy, sortDirection, searchColumn, searchValue);
    }
    
    
    // The base implementation that derived services will call (Option B)
    public virtual async Task<(List<TEntity> Items, int TotalCount)> GetWithCountAsync<TRequest>(TRequest request)
        where TRequest : BasePaginatedSortedRequest
    {
        var (page, pageSize, sortBy, sortDirection, searchColumn, searchValue) = GetPaginationParams(request);
        
        var (items, total) = await Repository.GetAllPaginatedAsync(
            page,
            pageSize,
            sortBy,
            sortDirection,
            searchColumn,
            searchValue
        );
        

        return (items, total);
    }


    public virtual async Task<TEntity?> GetAsync(Guid id)
    {
        return await Repository.GetByIdAsync(id);
    }

    public virtual async Task<TEntity> UpdateAsync(TUpdateDto dto, TEntity entity)
    {
        Mapper.Map(dto, entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        Repository.Delete(entity);

        await Db.SaveChangesAsync();
    }

    #endregion
}