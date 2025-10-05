namespace Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    DateTime? DeletedAt { get; set; }
}