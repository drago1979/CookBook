namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    DateTime? DeletedAt { get; set; }
}