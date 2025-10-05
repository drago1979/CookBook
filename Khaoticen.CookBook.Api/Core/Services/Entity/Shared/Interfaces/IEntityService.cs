namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;

public interface IEntityService<TEntity, TCreateDto, TUpdateDto>  // todo: contravariant?
    where TEntity : class
{
    public TEntity Create(TCreateDto dto);
    public Task<TEntity> CreateAsync(TCreateDto dto);
    public Task<TEntity?> Get(Guid id);
    public Task<List<TEntity>> GetAll();
    public Task<TEntity> Update(TEntity entity, TUpdateDto dto);
    public Task Delete(TEntity entity);
}