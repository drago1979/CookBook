namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

public class BaseEntityResponseDto
{
    public required Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } // todo: user-preview-format
    public DateTime? UpdatedAt { get; set; } // todo: user-preview-format
}