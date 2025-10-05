using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Entities;

// [Index(nameof(Name), IsUnique = true)]
public class Category : BaseEntity
{
//     [MaxLength(100)]
//     public required string Name { get; set; }

    // public ICollection<Recipe> Recipes { get; set; } // todo: treba li? ; nullable;
}







