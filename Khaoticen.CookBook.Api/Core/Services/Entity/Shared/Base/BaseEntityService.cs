using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;

public class BaseEntityService<
    TEntity,
    TEntityFactory,
    TCreateDto,
    TUpdateDto
>
    where TEntity : BaseEntity
    where TEntityFactory : BaseFactory<TEntity, TCreateDto, TUpdateDto>
{
    protected readonly AppDbContext Db;
    protected readonly TEntityFactory Factory;

    public BaseEntityService(AppDbContext db, TEntityFactory factory)
    {
        Db = db;
        Factory = factory;
    }

    // public virtual TEntity Create(TCreateDto dto)
    // {
    //     var entity = Factory.Create(dto);
    //
    //     Db.Set<TEntity>().Add(entity);
    //
    //     return entity;
    // }

    // public virtual async Task<TEntity> CreateAndSave(TCreateDto dto)
    // {
    //     var entity = Create(dto);
    //     
    //     await Db.SaveChangesAsync();
    //
    //     return entity;
    // }

    // public virtual async Task<TEntity?> Get(Guid id)
    // {
    //     return await Db.Set<TEntity>().FindAsync(id);
    // }

    // public virtual async Task<List<TEntity>> GetAll()
    // {
    //     return await Db.Set<TEntity>().ToListAsync();
    // }

    // public virtual async Task<TEntity> Update(TEntity entity, TUpdateDto dto)
    // {
    //     Factory.Update(dto, entity);
    //
    //     await Db.SaveChangesAsync();
    //
    //     return entity;
    // }

//     public virtual async Task Delete(TEntity entity) // todo!!! : move to concrete service or pull evrthng here
//     {
//         Db.Set<TEntity>().Remove(entity);
//
//         await Db.SaveChangesAsync();
//     }
}