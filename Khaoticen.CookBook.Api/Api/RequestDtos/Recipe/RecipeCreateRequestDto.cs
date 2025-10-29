using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;

public class RecipeCreateRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 10)]
    public required string Title { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public required  string Description { get; set; }
    
    public required RecipeCategoriesUpdateRequestDto Categories { get; set; }
    
    [StringLength(100, MinimumLength = 10)]
    public string? ImageUrl { get; set; }
}