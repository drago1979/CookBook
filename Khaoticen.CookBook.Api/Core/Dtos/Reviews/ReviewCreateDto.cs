using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Reviews;

public class ReviewCreateDto
{
    [MaxLength(100)]
    public required  string Comment { get; set; }
    
    
    // public required  Recipe Recipe { get; set; } // todo: check?
    
    // public Recipe Recipe { get; set; } // todo: needed?
}