using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Validation;

namespace Khaoticen.CookBook.Api.Api.Dtos.Request.Recipe;

public class CategoriesUpdateRecipeRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one category ID must be provided.")]
    [StringGuidCollection]
    public required List<string> CategoryIds { get; set; }
}