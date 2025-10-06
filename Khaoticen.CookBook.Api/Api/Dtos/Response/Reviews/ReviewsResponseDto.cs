using Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Reviews;

public class ReviewsResponseDto: BaseEntitiesResponseDto
{
    public required string Comment { get; init; }
    public required Guid RecipeId { get; init; }
}