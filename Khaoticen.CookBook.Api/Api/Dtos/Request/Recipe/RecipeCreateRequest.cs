using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Validation;

namespace Khaoticen.CookBook.Api.Api.Dtos.Request.Recipe;

public class RecipeCreateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 10)]
    public required string Title { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public required  string Description { get; set; }
    
    public required CategoriesUpdateRecipeRequest Categories { get; set; }
    
    [StringLength(100, MinimumLength = 10)]
    public string? ImageUrl { get; set; } // todo: finish this
}