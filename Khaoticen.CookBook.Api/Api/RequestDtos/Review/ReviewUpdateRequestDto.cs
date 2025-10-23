using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Review;

public class ReviewUpdateRequestDto
{
    [StringLength(100, MinimumLength = 10)]
    public required string Comment { get; set; }
}