using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Entities.Entity;

namespace Khaoticen.CookBook.Api.Core.Entities;

public class Review : BaseEntity
{
    
    [MaxLength(100)]
    public required  string Comment { get; set; }
    
    public required  Recipe Recipe { get; set; }
    
    // public Recipe Recipe { get; set; } // todo: needed?
}