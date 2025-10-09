using System.ComponentModel.DataAnnotations;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;

public class ReviewCreateDto
{
    public required string Comment { get; set; }
}