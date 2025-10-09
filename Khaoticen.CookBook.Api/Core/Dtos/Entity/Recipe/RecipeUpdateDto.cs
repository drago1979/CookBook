using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;

public class RecipeUpdateDto
{
    public required string Title { get; set; }
    
    public required string Description { get; set; }
    
    public string? ImageUrl { get; set; } // todo: finish this
}