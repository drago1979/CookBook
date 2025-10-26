using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Category;

public class CategoriesPaginatedResponseDto<TCategoryInListResponseDto>
    : BasePaginatedResponse<TCategoryInListResponseDto>
    where TCategoryInListResponseDto : BaseEntityInListResponseDto;