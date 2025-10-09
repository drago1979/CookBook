using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.Dtos.Request.Category;

public class CategoryCreateRequest
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public required string Name { get; set; }
    
    // public ICollection<Recipe> Recipes { get; set; } // todo: treba li? ; nullable;
}