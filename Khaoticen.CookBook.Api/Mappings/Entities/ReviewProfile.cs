using Khaoticen.CookBook.Api.Api.Dtos.Request.Review;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Reviews;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Mappings.Entities;

public class ReviewProfile: BaseEntityProfile<
    Review,
    ReviewCreateRequest,
    ReviewUpdateRequest,
    ReviewCreateDto, 
    ReviewUpdateDto,
    ReviewResponseDto,
    ReviewsResponseDto
>;