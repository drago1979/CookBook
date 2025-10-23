namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
}