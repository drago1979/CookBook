using Khaoticen.CookBook.Api.Api.Dtos.Base;

namespace Khaoticen.CookBook.Api.Api.DTOs;

public class RecipeResponseDto : BaseResponseDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? Url { get; set; }
}