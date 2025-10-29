using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Khaoticen.CookBook.Api.Infrastructure.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Intercepts the SaveChanges operation asynchronously to apply audit information (e.g., tracking CreatedAt and UpdatedAt timestamps) to entities derived from BaseEntity.
    /// </summary>
    /// <param name="eventData">Provides contextual information about the SaveChanges operation being executed.</param>
    /// <param name="result">The interception result that can be modified or used to determine the result of the SaveChanges operation.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A ValueTask representing the asynchronous operation, potentially with a modified result.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not { } context)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}