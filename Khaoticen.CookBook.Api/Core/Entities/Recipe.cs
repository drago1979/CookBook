using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;


namespace Khaoticen.CookBook.Api.Core.Entities;

[Table("Recipes")]
public class Recipe : BaseSoftDeletableEntity
{
    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(1000)]
    public required string Description { get; set; }

    [MaxLength(200)]
    public string? ImageUrl { get; set; }
    
    public ICollection<Category> Categories { get; } = [];
    public ICollection<Review> Reviews { get; set; } = new List<Review>(); // todo!!: proveri
    

    public override void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;

        foreach (var review in Reviews)
        {
            review.SoftDelete();
        }
    }

    

    //
    // // 
    // public ICollection<Review>? Reviews { get; set; } // 12M // todo: nullable?
}