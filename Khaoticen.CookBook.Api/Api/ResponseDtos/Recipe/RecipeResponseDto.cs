using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;

public class RecipeResponseDto : BaseEntityResponseDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? ImageUrl { get; init; }
    public List<ReviewInRelatedResponseDto> Reviews { get; init; } = [];
    public List<CategoryInRelatedResponseDto> Categories { get; init; } = [];
}