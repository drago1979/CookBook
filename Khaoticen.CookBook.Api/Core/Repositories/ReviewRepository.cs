using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class ReviewRepository(AppDbContext db, IReviewQueryMetadata metadata)
    : BaseRepository<Review>(db, metadata)
{
    /// <summary>
    /// Retrieves a review by its unique identifier, including all related entities.
    /// </summary>
    /// <param name="id">The unique identifier of the review to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the review with related entities if found; otherwise, null.
    /// </returns>
    public async Task<Review?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await Table
            .Include(r => r.Recipe)
            .FirstOrDefaultAsync(r => r.Id == id);

    /// Retrieves a list of all entities, including those marked as deleted.
    /// This method bypasses any query filters that normally exclude soft-deleted entities
    /// to return the full list of records from the database.
    /// <returns>A list of all entities, including softly deleted ones.</returns>
    public async Task<List<Review>> GetAllWithDeletedAsync() =>
        await Table.IgnoreQueryFilters().ToListAsync();
}