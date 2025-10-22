using AutoMapper;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

namespace Khaoticen.CookBook.Api.Shared.Mappings.Entities.Shared;

public abstract class BaseEntityProfile<
    TEntity,
    TCreateRequest,
    TUpdateRequest,
    TCreateDto, 
    TUpdateDto,
    TEntityResponseDto,
    TEntitiesResponseDto
>: Profile
    where TEntity : BaseEntity
    where TCreateRequest : class // todo: IF?
    where TUpdateRequest : class // todo: IF?
    where TCreateDto : class // todo: IF?
    where TUpdateDto : class // todo: IF?
    where TEntityResponseDto: class // todo: IF?
{
    protected BaseEntityProfile()
    {
        // REQUESTS => DTOs
        CreateMap<TCreateRequest, TCreateDto>();
        CreateMap<TUpdateRequest, TUpdateDto>();
        
        // DTOs => ENTITIES
        CreateMap<TCreateDto, TEntity>();
        CreateMap<TUpdateDto, TEntity>();
        
        // ENTITIES => RESPONSES
        CreateMap<TEntity, TEntityResponseDto>();
        CreateMap<TEntity, TEntitiesResponseDto>();
    }
}