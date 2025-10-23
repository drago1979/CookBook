using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;

public class RecipeInRelatedResponseDto : BaseEntityInRelatedResponseDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? ImageUrl { get; init; }
}