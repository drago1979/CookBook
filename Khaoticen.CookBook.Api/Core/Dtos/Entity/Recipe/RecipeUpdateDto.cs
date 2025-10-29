using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;

public class RecipeUpdateDto: IUpdateDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
}