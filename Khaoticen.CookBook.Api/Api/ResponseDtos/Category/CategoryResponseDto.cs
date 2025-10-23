using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Category;

public class CategoryResponseDto: BaseEntityResponseDto
{
    public required string Name { get; init; }
    public List<RecipeInRelatedResponseDto> Recipes { get; init; } = [];
}