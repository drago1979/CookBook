using Khaoticen.CookBook.Api.Api.Dtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response;

public class ReviewResponseDto: BaseResponseDto
{
    public required string Comment { get; init; }
    
    // public required Recipe Recipe { get; set; }
}