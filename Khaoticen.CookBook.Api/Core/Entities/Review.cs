using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

namespace Khaoticen.CookBook.Api.Core.Entities;

public class Review : BaseEntity
{
    [MaxLength(100)]
    public required string Comment { get; set; }

    public required Guid RecipeId { get; set; }
    public required Recipe Recipe { get; set; } 
    
    // public required  Recipe Recipe { get; set; }

    // public Recipe Recipe { get; set; } // todo: needed?
}