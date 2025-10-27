using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class ReviewRepository(AppDbContext db, IReviewQueryMetadata metadata) : BaseRepository<Review>(db, metadata)
{
    public async Task<Review?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await Table
            .Include(r => r.Recipe)
            .FirstOrDefaultAsync(r => r.Id == id);
    
    public async Task<List<Review>> GetAllWithDeletedAsync() =>
        await Table.IgnoreQueryFilters().ToListAsync();
}