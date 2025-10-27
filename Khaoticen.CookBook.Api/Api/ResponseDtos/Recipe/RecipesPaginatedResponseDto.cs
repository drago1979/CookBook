using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;

public class RecipesPaginatedResponseDto<TResponseInListResponseDto>
    : BasePaginatedResponse<TResponseInListResponseDto>
    where TResponseInListResponseDto : BaseEntityInListResponseDto;