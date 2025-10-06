using AutoMapper;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;

public abstract class AppControllerBase<TEntity, TResponseDto>(IMapper mapper) : ControllerBase
    where TEntity : BaseEntity
    where TResponseDto : class
{
    protected readonly IMapper Mapper = mapper;

    #region ServiceMethods

    protected List<TResponseDto> Transform(IEnumerable<TEntity> entities)
    {
        return entities.Select(Transform).ToList();
    }

    protected TResponseDto Transform(TEntity entity)
    {
        return Mapper.Map<TResponseDto>(entity);
    }

    #endregion
}