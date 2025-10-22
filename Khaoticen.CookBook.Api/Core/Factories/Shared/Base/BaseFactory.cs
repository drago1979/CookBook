using AutoMapper;

namespace Khaoticen.CookBook.Api.Core.Factories.Shared.Base;

public abstract class BaseFactory<TEntity, TCreateDto, TUpdateDto>(IMapper mapper)
{
    public virtual TEntity Create(TCreateDto dto)
    {
        return mapper.Map<TEntity>(dto);
    }

    // TODO: Domain rules / overrides
    // recipe.Title = dto.Title.Trim();          // normalization
    // recipe.CreatedAt = DateTime.UtcNow;       // system-set field
    
}