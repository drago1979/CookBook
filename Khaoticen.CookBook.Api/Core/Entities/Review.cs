using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Entities;

[Index(nameof(Nickname))]
public class Review : BaseSoftDeletableEntity
{
    [MaxLength(100)]
    public required string Nickname { get; set; }
    
    [MaxLength(100)]
    public required string Comment { get; set; }

    public required Guid RecipeId { get; set; }
    
    public required Recipe Recipe { get; set; }
}