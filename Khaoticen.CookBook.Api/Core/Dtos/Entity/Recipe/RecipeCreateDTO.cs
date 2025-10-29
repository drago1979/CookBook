using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;

public class RecipeCreateDto: ICreateDto
{
    public required string Title { get; set; }
    public required  string Description { get; set; }
    public required RecipeCategoriesDto Categories { get; set; }
    public string? ImageUrl { get; set; } 
}