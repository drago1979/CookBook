namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

public class BaseEntityResponseDto
{
    public required Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}