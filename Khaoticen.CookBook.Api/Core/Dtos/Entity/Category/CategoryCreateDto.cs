using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;

public class CategoryCreateDto
{
    
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public required string Name { get; set; }

    // public ICollection<Recipe> Recipes { get; set; } // todo: treba li? ; nullable;
}