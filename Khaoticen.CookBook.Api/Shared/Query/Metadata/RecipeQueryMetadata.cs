using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Shared.Query.Metadata;

public class RecipeQueryMetadata : IRecipeQueryMetadata
{
    public IReadOnlyDictionary<string, Expression<Func<Recipe, object>>> Sorts { get; } =
        new Dictionary<string, Expression<Func<Recipe, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = r => r.Id,
            ["createdAt"] = r => r.CreatedAt,
            ["title"] = r => r.Title
        };


    public IReadOnlyDictionary<string, Expression<Func<Recipe, string>>> Filters { get; } =
        new Dictionary<string, Expression<Func<Recipe, string>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["title"] = r => r.Title
        };
}