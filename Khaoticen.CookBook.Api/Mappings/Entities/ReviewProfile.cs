using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Mappings.Entities;

public class ReviewProfile: BaseEntityProfile<
    Review,
    ReviewCreateRequestDto,
    ReviewUpdateRequestDto,
    ReviewCreateDto, 
    ReviewUpdateDto,
    ReviewResponseDto,
    ReviewsResponseDto
>;