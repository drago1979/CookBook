using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;

public class ReviewCreateDto: ICreateDto
{
    public required string Comment { get; set; }
}