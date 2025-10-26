using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Shared.Query.Metadata;

public class CategoryQueryMetadata
{
    public static readonly Dictionary<string, Expression<Func<Category, object>>> Sorts =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = c => c.Name,
            ["createdAt"] = c => c.CreatedAt,
        };
    
    public static readonly Dictionary<string, Expression<Func<Category, string>>> Filters =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = c => c.Name,
        };
    
}