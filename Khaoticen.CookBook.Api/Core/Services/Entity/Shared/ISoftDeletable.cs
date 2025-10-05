namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    DateTime? DeletedAt { get; set; }
}