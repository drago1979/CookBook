using AutoMapper;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Mappings.Entities.Shared.Base;

public abstract class BaseEntityProfile<TEntity, TResponseDto, TCreateDto, TUpdateDto> : Profile
    where TEntity : BaseEntity
    where TResponseDto: class // todo: IF?
    where TCreateDto : class // todo: IF?
    where TUpdateDto : class // todo: IF?
{
    protected BaseEntityProfile()
    {
        CrudMapping();
        ApiResponseMapping();
    }

    private void CrudMapping()
    {
        CreateMap<TCreateDto, TEntity>();
        CreateMap<TUpdateDto, TEntity>();
    }
    
    private void ApiResponseMapping()
    {
        CreateMap<TEntity, TResponseDto>();
    }
}