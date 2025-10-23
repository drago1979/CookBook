using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Category;

public class CategoriesResponseDto: BaseEntitiesResponseDto
{
    public required string Name { get; set; } 
    public List<RecipeInRelatedResponseDto> Recipes { get; set; } = [];
}