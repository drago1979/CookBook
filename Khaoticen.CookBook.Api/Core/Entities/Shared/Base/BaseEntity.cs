using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

[Index(nameof(CreatedAt))]
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}