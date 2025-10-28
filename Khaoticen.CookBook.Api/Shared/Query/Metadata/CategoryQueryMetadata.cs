using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Shared.Query.Metadata;

public class CategoryQueryMetadata : ICategoryQueryMetadata
{
    public IReadOnlyDictionary<string, Expression<Func<Category, object>>> Sorts { get; } =
        new Dictionary<string, Expression<Func<Category, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = c => c.Id,
            ["name"] = c => c.Name,
            ["createdAt"] = c => c.CreatedAt
        };

    public IReadOnlyDictionary<string, Expression<Func<Category, string>>> Filters { get; } =
        new Dictionary<string, Expression<Func<Category, string>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = c => c.Name
        };
}