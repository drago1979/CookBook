using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;

public class CategoryCreateDto
{
    
    public required string Name { get; set; }
    
}