using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class ReviewRepository(AppDbContext db) : BaseRepository<Review>(db)
{
    public async Task<List<Review>> GetAllWithDeletedAsync() =>
        await Table.IgnoreQueryFilters().ToListAsync();
}