using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.Dtos.Request.Review;

public class ReviewCreateRequest
{
    [StringLength(100, MinimumLength = 10)]
    public required  string Comment { get; set; }
}