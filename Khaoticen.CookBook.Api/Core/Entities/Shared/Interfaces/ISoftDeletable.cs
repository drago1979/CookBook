namespace Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

public interface ISoftDeletable
{
    DateTimeOffset? DeletedAt { get; set; }
}