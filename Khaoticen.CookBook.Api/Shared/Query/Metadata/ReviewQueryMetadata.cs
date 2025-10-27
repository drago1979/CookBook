using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;

namespace Khaoticen.CookBook.Api.Shared.Query.Metadata;

public class ReviewQueryMetadata: IReviewQueryMetadata
{
    public IReadOnlyDictionary<string, Expression<Func<Review, object>>> Sorts { get; } =
        new Dictionary<string, Expression<Func<Review, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["id"] = r => r.Id,
            ["createdAt"] = r => r.CreatedAt
        };
    
    public IReadOnlyDictionary<string, Expression<Func<Review, string>>>? Filters => null; // todo!!!: check this
}