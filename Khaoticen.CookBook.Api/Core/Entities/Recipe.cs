using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Core.Entities;

[Table("Recipes")] // todo: not needed?
public class Recipe : SoftDeletableEntity
{
    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(1000)]
    public required string Description { get; set; }

    [MaxLength(200)]
    public string? Url { get; set; }
    
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    
    
    

    //
    // // public ICollection<Category> Categories { get; set; } // M2M todo: obavezno
    // public ICollection<Review>? Reviews { get; set; } // 12M // todo: nullable?
}