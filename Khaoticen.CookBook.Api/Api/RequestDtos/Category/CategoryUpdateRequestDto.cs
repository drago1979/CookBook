using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Category;

public class CategoryUpdateRequestDto
{
    [StringLength(100, MinimumLength = 5)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public required string Name { get; set; }
}