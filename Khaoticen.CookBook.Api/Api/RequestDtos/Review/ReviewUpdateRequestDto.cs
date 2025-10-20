using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Review;

public class ReviewUpdateRequestDto
{
    [StringLength(100, MinimumLength = 10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // todo!! check
    public required  string Comment { get; set; }
}