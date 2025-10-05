using Khaoticen.CookBook.Api.Api.Dtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response;

public class RecipeResponseDto : BaseResponseDto
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string? Url { get; init; }
}