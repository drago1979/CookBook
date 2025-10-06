using Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Reviews;

public class ReviewResponseDto: BaseEntityResponseDto
{
    public required string Comment { get; init; }
    public required Guid RecipeId { get; init; }
    
    // public required Recipe Recipe { get; set; }
}