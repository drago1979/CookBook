using AutoMapper;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;

public abstract class BaseAppController<
    TEntity,
    TEntityResponseDto,
    TEntityInListResponseDto,
    TEntityPaginatedResponse,
    TEntityAllRequest
>(IMapper mapper) : ControllerBase
    where TEntity : BaseEntity
    where TEntityResponseDto : BaseEntityResponseDto
    where TEntityInListResponseDto : BaseEntityInListResponseDto
    where TEntityPaginatedResponse : BasePaginatedResponse<TEntityInListResponseDto>, new()
    where TEntityAllRequest : BasePaginatedSortedRequest
{
    protected readonly IMapper Mapper = mapper;
    
    protected TEntityResponseDto TransformEntityToResponse(TEntity entity) =>
        Mapper.Map<TEntityResponseDto>(entity);
    
    protected List<TEntityInListResponseDto> TransformEntitiesToResponse(IEnumerable<TEntity?> entities) =>
        Mapper.Map<List<TEntityInListResponseDto>>(entities);
    
    protected TEntityPaginatedResponse TransformToPaginated(
        TEntityAllRequest request,
        List<TEntity> items,
        int total
    )
    {
        var itemsDto = TransformEntitiesToResponse(items);

        var response = new TEntityPaginatedResponse
        {
            Items = itemsDto,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = total
        };

        return response;
    }
}