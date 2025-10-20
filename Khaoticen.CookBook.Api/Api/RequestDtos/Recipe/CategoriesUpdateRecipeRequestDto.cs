using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Validation;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;

public class CategoriesUpdateRecipeRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one category ID must be provided.")]
    [StringGuidCollection]
    public required List<string> CategoryIds { get; set; }
}