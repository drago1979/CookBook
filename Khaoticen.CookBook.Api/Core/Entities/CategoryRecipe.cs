namespace Khaoticen.CookBook.Api.Core.Entities;

public class CategoryRecipe
{
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public Guid RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;
}