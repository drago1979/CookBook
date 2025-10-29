using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;


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

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    public virtual async Task<TEntity?> GetAsync(Guid id)
    {
        return await Repository.GetByIdAsync(id);
    }

    /// <summary>
    /// Updates the specified entity based on the provided update DTO, maps the changes, and saves them to the database.
    /// </summary>
    /// <param name="dto">The data transfer object containing the updated information for the entity.</param>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>The updated entity.</returns>
    public virtual async Task<TEntity> UpdateAsync(TUpdateDto dto, TEntity entity)
    {
        Mapper.Map(dto, entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    /// <summary>
    /// Deletes the specified entity asynchronously, applying necessary logic specific to the entity's service implementation.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        Repository.Delete(entity);

        await Db.SaveChangesAsync();
    }
    
    #endregion
}