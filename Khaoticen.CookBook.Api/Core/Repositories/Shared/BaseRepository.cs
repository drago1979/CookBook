using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories.Shared;

public abstract class BaseRepository<TEntity>(AppDbContext db)
    where TEntity : BaseEntity
{
    protected DbSet<TEntity> Table => db.Set<TEntity>();

    public void Add(TEntity entity)
    {
        Table.Add(entity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Table.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await Table.ToListAsync();
    }

    public void Update(TEntity entity)
    {
        Table.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Table.Remove(entity);
    }
}