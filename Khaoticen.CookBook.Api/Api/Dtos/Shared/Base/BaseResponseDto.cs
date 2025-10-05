namespace Khaoticen.CookBook.Api.Api.Dtos.Shared.Base;

public class BaseResponseDto
{
    public required Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } // todo: user-preview-format
    public DateTime? UpdatedAt { get; set; } // todo: user-preview-format
    
    // public Guid CreatedBy { get; set; } // todo: check - relationship?
    // public Guid UpdatedBy { get; set; }  // todo: check - relationship?
}