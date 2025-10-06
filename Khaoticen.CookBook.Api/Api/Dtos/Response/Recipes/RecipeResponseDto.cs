using Khaoticen.CookBook.Api.Api.Dtos.Response.Reviews;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Recipes;

public class RecipeResponseDto : BaseEntityResponseDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? Url { get; init; }
    public List<ReviewResponseDto> Reviews { get; init; } = new();
}