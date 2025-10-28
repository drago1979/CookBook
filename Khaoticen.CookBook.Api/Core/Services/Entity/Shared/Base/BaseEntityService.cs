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