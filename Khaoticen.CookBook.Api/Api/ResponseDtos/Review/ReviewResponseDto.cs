using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Review;

public class ReviewResponseDto: BaseEntityResponseDto
{
    public required string Comment { get; init; }
    public required Guid RecipeId { get; init; }
}