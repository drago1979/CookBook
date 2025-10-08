using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Entities;

[Table("Categories")]
[Index(nameof(Name), IsUnique = true)]
public class Category : BaseEntity
{
     [MaxLength(100)]
     public required string Name { get; set; }
     
     public ICollection<Recipe> Recipes { get; } = []; // todo!!!: Cascading? (soft delete?)
}



