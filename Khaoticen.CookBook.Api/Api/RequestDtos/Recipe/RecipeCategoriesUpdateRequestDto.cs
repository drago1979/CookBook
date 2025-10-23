using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Api.ValidationAttributes;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;

public class RecipeCategoriesUpdateRequestDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one category ID must be provided.")]
    [StringGuidCollection]
    public required List<string> CategoryIds { get; set; }
}