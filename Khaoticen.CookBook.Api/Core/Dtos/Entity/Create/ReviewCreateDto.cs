using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Create;

public class ReviewCreateDto
{
    [StringLength(100, MinimumLength = 10)]
    public required  string Comment { get; set; }
    
    
    // public required  Recipe Recipe { get; set; } // todo: check?
    
    // public Recipe Recipe { get; set; } // todo: needed?
}