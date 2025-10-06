namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

public class BaseEntityResponseDto
{
    public required Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } // todo: user-preview-format
    public DateTime? UpdatedAt { get; set; } // todo: user-preview-format
    
    // public Guid CreatedBy { get; set; } // todo: check if needed
    // public Guid UpdatedBy { get; set; }  // todo: check if needed
}