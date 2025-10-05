using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Khaoticen.CookBook.Api.Core.Entities.Entity;

namespace Khaoticen.CookBook.Api.Core.Entities;

[Table("Recipes")]
public class Recipe : SoftDeletableEntity
{
    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(1000)]
    public required string Description { get; set; }

    [MaxLength(200)]
    public string? Url { get; set; }
    
    
    

    //
    // // public ICollection<Category> Categories { get; set; } // M2M todo: obavezno
    // public ICollection<Review>? Reviews { get; set; } // 12M // todo: nullable?
}