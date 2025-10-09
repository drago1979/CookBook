using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;

public class RecipeCreateDto
{
    public required string Title { get; set; }
    
    public required  string Description { get; set; }

    public required RecipeCategoriesDto Categories { get; set; }
    
    [StringLength(100, MinimumLength = 10)]
    public string? ImageUrl { get; set; } // todo: finish this
}