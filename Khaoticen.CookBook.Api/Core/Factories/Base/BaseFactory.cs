using AutoMapper;

namespace Khaoticen.CookBook.Api.Core.Factories.Base;

public abstract class BaseFactory<TEntity, TCreateDto, TUpdateDto>(IMapper mapper)
{
    public TEntity Create(TCreateDto dto)
    {
        return mapper.Map<TEntity>(dto);
    }

    // TODO: Domain rules / overrides
    // recipe.Title = dto.Title.Trim();          // normalization
    // recipe.CreatedAt = DateTime.UtcNow;       // system-set field

    public void Update(TUpdateDto dto, TEntity entity)
    {
        mapper.Map(dto, entity);

        // Domain rules / overrides
        // entity.Title = dto.Title.Trim();          // normalization
        // entity.CreatedAt = DateTime.UtcNow;       // system-set field
    }
}