using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;

public class CategoryUpdateDto
{
    public required string Name { get; set; }
}