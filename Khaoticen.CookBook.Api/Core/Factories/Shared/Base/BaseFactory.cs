using AutoMapper;

namespace Khaoticen.CookBook.Api.Core.Factories.Shared.Base;

public abstract class BaseFactory<TEntity, TCreateDto>(IMapper mapper)
{
    public virtual TEntity Create(TCreateDto dto)
    {
        return mapper.Map<TEntity>(dto);
    }
}