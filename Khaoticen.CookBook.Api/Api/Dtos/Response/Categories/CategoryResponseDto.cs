using Khaoticen.CookBook.Api.Api.Dtos.Response.Recipes;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Categories;

public class CategoryResponseDto: BaseEntityResponseDto
{
    public required string Name { get; set; }
    public List<RecipeResponseDto> Recipes { get; init; } = []; // todo!! Circular ref?
}