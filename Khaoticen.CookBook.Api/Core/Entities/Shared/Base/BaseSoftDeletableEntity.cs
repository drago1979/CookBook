using Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

public class BaseSoftDeletableEntity : BaseEntity, ISoftDeletable // todo!!!: nije bilo dovoljno po base-class?
{
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted => DeletedAt.HasValue; // my version

    public virtual void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
    }

    // bool ISoftDeletable.IsDeleted { get; set; } // enforced after implementing interface
}

//     public void SoftDelete() => DeletedAt = DateTime.UtcNow; // Change to delete?