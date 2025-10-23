using AutoMapper;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;

public abstract class AppControllerBase<
    TEntity,
    TEntityResponseDto,
    TEntitiesResponseDto
>(IMapper mapper) : ControllerBase
    where TEntity : BaseEntity
    where TEntityResponseDto : BaseEntityResponseDto
    where TEntitiesResponseDto : BaseEntitiesResponseDto
{
    protected readonly IMapper Mapper = mapper;

    #region HELPERS

    protected List<TEntitiesResponseDto> TransformEntitiesToResponse(IEnumerable<TEntity?> entities) =>
        Mapper.Map<List<TEntitiesResponseDto>>(entities);

    protected TEntityResponseDto TransformEntityToResponse(TEntity entity) =>
        Mapper.Map<TEntityResponseDto>(entity);

    #endregion
}