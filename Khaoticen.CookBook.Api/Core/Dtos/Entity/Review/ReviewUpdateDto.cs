using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;

public class ReviewUpdateDto
{
    public required  string Comment { get; set; }
    
    
    // public required  Recipe Recipe { get; set; } // todo: check?
    
    // public Recipe Recipe { get; set; } // todo: needed?
}