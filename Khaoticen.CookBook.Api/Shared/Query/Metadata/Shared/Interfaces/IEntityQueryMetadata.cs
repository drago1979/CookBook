using System.Linq.Expressions;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;

namespace Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;

public interface IEntityQueryMetadata<TEntity>
    where TEntity : BaseEntity
{
    IReadOnlyDictionary<string, Expression<Func<TEntity, object>>> Sorts { get; }
    IReadOnlyDictionary<string, Expression<Func<TEntity, string>>>? Filters { get; }
}

// todo: note-for-self: diff compared to prev
// public static abstract IReadOnlyDictionary<string, Expression<Func<TEntity, object>>> Sorts { get; } 
// static virtual IReadOnlyDictionary<string, Expression<Func<TEntity, object>>>? Filters => null;