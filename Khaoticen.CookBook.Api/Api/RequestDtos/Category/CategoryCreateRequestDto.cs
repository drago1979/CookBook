using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Category;

public class CategoryCreateRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 5)]
    public required string Name { get; set; }
}