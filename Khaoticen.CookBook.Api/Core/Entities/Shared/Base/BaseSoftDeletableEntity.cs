using Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

public abstract class BaseSoftDeletableEntity : BaseEntity, ISoftDeletable
{
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}