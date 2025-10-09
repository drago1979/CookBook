using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Khaoticen.CookBook.Api.Core.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;

public class RecipeCreateDto
{
    public required string Title { get; set; }
    
    public required  string Description { get; set; }
    
    public required Guid CategoryId { get; set; }
    
    [StringLength(100, MinimumLength = 10)]
    public string? ImageUrl { get; set; } // todo: finish this
}


// public class RecipeCreateDto
// {
//
//     [Required]
//     [StringLength(100, MinimumLength = 10)]
//     public required string Title { get; set; }
//
//     [Required]
//     [StringLength(100, MinimumLength = 10)]
//     public required  string Description { get; set; }
//     
//     [Required]
//     [StringGuid]
//     public required string CategoryId { get; set; }
//
//     // public Guid? CategoryGuidOrNull
//     // {
//     //     get
//     //     {
//     //         if (Guid.TryParse(CategoryId, out var g)) return g;
//     //         return null;
//     //     }
//     // }
//     
//     // public Guid CategoryGuid => Guid.Parse(CategoryId);
//     
//     
//     [StringLength(100, MinimumLength = 10)]
//     public string? ImageUrl { get; set; } // todo: finish this
// }