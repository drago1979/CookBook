using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

[Index(nameof(CreatedAt))]
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}