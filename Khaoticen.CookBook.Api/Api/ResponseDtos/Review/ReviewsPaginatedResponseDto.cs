using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Review;

public class ReviewsPaginatedResponseDto<TReviewInListResponseDto>
    : BasePaginatedResponse<TReviewInListResponseDto>
    where TReviewInListResponseDto : BaseEntityInListResponseDto;