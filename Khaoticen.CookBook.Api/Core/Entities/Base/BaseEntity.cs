namespace Khaoticen.CookBook.Api.Core.Entities.Entity;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // public Guid CreatedBy { get; set; } // todo: check - relationship?
    // public Guid UpdatedBy { get; set; }  // todo: check - relationship?
}