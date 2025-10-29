using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.EntityFrameworkCore;


namespace Khaoticen.CookBook.Api.Core.Entities;

[Table("Recipes")]
[Index(nameof(Title))]
public class Recipe : BaseSoftDeletableEntity
{
    [MaxLength(100)]
    public required string Title { get; init; }

    [MaxLength(1000)]
    public required string Description { get; init; }

    [MaxLength(200)]
    public string? ImageUrl { get; init; }
    
    public ICollection<Category> Categories { get; init; } = [];

    public ICollection<Review> Reviews { get; init; } = [];
    

    public override void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;

        foreach (var review in Reviews)
        {
            review.SoftDelete();
        }
    }
}