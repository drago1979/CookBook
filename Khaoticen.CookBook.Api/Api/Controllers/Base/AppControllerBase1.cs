using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Base;

// public abstract class AppControllerBase1(IMapper mapper) : ControllerBase
public abstract class AppControllerBase1<TEntity, TResponseDto>(IMapper mapper) : ControllerBase
{
    // protected TResponseDto Transform(TEntity entity)
    // {
    //     return mapper.Map<TResponseDto>(entity);
    // }
    //
    // protected List<TResponseDto>Transform(IEnumerable<TEntity> entities)
    // {
    //     return entities.Select(Transform).ToList();
    // }
    //
    //
    // // protected List<BaseResponseDto> Transform<T>(IEnumerable<T> entities)
    // // {
    // //
    // //     return entities.Select(Transform).ToList();
    // // }
}