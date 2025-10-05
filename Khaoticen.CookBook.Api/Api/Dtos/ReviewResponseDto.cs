using Khaoticen.CookBook.Api.Api.Dtos.Base;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Api.DTOs;

public class ReviewResponseDto: BaseResponseDto
{
    public required string Comment { get; set; }
    
    // public required Recipe Recipe { get; set; }
}