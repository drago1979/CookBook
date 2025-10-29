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
    where TCreateRequest : class
    where TUpdateRequest : class
    where TCreateDto : class
    where TUpdateDto : class
    where TEntityResponseDto: class
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