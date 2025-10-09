using AutoMapper;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;

public abstract class AppControllerBase<
    TEntity,
    TEntityResponseDto,
    TEntitiesResponseDto
>
    (IMapper mapper) : ControllerBase
    where TEntity : BaseEntity
    where TEntityResponseDto : class
    where TEntitiesResponseDto : class
{
    protected readonly IMapper Mapper = mapper;

    #region ServiceMethods

    protected List<TEntitiesResponseDto> TransformEntitiesToResponse(IEnumerable<TEntity> entities)
    {
        return Mapper.Map<List<TEntitiesResponseDto>>(entities);
    }

    protected TEntityResponseDto TransformEntityToResponse(TEntity entity)
    {
        return Mapper.Map<TEntityResponseDto>(entity);
    }

    #endregion
}