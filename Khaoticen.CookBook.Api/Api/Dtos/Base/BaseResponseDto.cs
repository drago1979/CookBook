namespace Khaoticen.CookBook.Api.Api.Dtos.Base;

public class BaseResponseDto
{
    public required Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // public Guid CreatedBy { get; set; } // todo: check - relationship?
    // public Guid UpdatedBy { get; set; }  // todo: check - relationship?
}