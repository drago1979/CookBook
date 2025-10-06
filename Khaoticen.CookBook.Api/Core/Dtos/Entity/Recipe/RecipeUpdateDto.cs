using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;

public class RecipeUpdateDto
{
    [StringLength(100, MinimumLength = 10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string Title { get; set; }

    [StringLength(100, MinimumLength = 10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string Description { get; set; }

    [StringLength(100, MinimumLength = 10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ImageUrl { get; set; } // todo: finish this
}