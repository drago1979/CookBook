using Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

public class BaseSoftDeletableEntity : BaseEntity, ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted => DeletedAt.HasValue; // my version
    

    // bool ISoftDeletable.IsDeleted { get; set; } // enforced after implementing interface
}

//     public void SoftDelete() => DeletedAt = DateTime.UtcNow; // Change to delete?