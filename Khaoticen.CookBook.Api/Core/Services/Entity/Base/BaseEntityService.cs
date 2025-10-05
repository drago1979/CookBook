using Khaoticen.CookBook.Api.Core.Dtos.Recipes;
using Khaoticen.CookBook.Api.Core.Entities.Entity;
using Khaoticen.CookBook.Api.Core.Factories.Base;
using Khaoticen.CookBook.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Base;

public class BaseEntityService<
    TEntity,
    TEntityFactory,
    TCreateDto,
    TUpdateDto
>(AppDbContext db, TEntityFactory factory)
    where TEntity : BaseEntity
    where TEntityFactory : BaseFactory<TEntity,TCreateDto,TUpdateDto>
{
    public async Task<TEntity> Create(TCreateDto dto)
    {
        var entity = factory.Create(dto);

        db.Set<TEntity>().Add(entity);
        await db.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity?> Get(Guid id)
    {
        return await db.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await db.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Update(TEntity entity, TUpdateDto dto)
    {
        factory.Update(dto, entity);
        
        await db.SaveChangesAsync();

        return entity;
    }

    public async Task Delete(TEntity entity)
    {
        db.Set<TEntity>().Remove(entity);

        await db.SaveChangesAsync();
    }
}