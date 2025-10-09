using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Khaoticen.CookBook.Api.Api.Dtos.Request.Review;

public class ReviewUpdateRequest
{
    [StringLength(100, MinimumLength = 10)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // todo!! check
    public required  string Comment { get; set; }
}