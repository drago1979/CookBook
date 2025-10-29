using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Repositories.Shared;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Query.Metadata.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Repositories;

public class CategoryRepository(AppDbContext db, ICategoryQueryMetadata metadata) : 
    BaseRepository<Category>(db, metadata)
{
    /// <summary>
    /// Fetches a category entity by its name asynchronously.
    /// </summary>
    /// <param name="name">The name of the category to retrieve.</param>
    /// <returns>A <see cref="Category"/> entity if found; otherwise, null.</returns>
    public async Task<Category?> GetByNameAsync(string name) 
        => await Table.FirstOrDefaultAsync(c => c.Name == name);

    /// <summary>
    /// Retrieves a category entity based on the given ID and includes all related entities such as recipes
    /// and their associated categories.
    /// </summary>
    /// <param name="id">The unique identifier of the category to retrieve.</param>
    /// <returns>The category entity with its related entities, or null if no category is found with the specified ID.</returns>
    public async Task<Category?> GetByIdIncludeAllRelatedAsync(Guid id) =>
        await Table
            .Include(c => c.Recipes)
            .ThenInclude(r => r.Categories)
            .FirstOrDefaultAsync(r => r.Id == id);

    /// <summary>
    /// Retrieves a list of categories based on the provided collection of category IDs.
    /// </summary>
    /// <param name="ids">The collection of unique identifiers for the categories to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of categories matching the specified IDs.</returns>
    public async Task<List<Category>> GetAllByIdsAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.Distinct().ToList();

        if (idList.Count == 0)
            return [];

        return await Table
            .Where(c => idList.Contains(c.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a list of existing category IDs from the database that match the provided IDs.
    /// </summary>
    /// <param name="ids">A collection of category IDs to check for existence in the database.</param>
    /// <returns>A list of GUIDs representing the IDs that exist in the database.</returns>
    public async Task<List<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
            return [];

        return await Table
            .Where(c => idList.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync();
    }
}