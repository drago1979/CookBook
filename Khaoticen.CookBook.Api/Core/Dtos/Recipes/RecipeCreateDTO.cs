using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Dtos.Recipes;

public class RecipeCreateDto
{
    [Required]
    [StringLength(100, MinimumLength = 10)]
    public required string Title { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 10)]
    public required  string Description { get; set; }

    [StringLength(100, MinimumLength = 10)]
    public string? Url { get; set; } // todo: finish this



    // public ICollection<Category> Categories { get; set; } // M2M todo: obavezno
    public ICollection<Review>? Reviews { get; set; } // 12M // todo: nullable?
}