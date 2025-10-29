using AutoMapper;
using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class ReviewService(
    AppDbContext db,
    ReviewFactory factory,
    ReviewRepository repository,
    IMapper mapper
)
    : BaseEntityService<
        Review,
        ReviewRepository,
        ReviewFactory,
        ReviewCreateDto,
        ReviewUpdateDto
    >(db, mapper, repository, factory), IReviewService
{
    #region CRUD

    /// <summary>
    /// Retrieves a review with all its related entities asynchronously by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the review with all its related entities if found; otherwise, null.
    /// </returns>
    public async Task<Review?> GetWithRelatedAsync(Guid id) =>
        await Repository.GetByIdIncludeAllRelatedAsync(id);

    /// Retrieves a list of all review entities, including those that have been soft-deleted.
    /// This method is useful for administrative purposes where visibility of deleted records is required.
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all review entities, including soft-deleted ones.</returns>
    public async Task<List<Review>> GetAllWithDeletedAsync() =>
        await Repository.GetAllWithDeletedAsync();

    /// <summary>
    /// Retrieves a paginated list of reviews along with the total count of reviews that match the specified criteria.
    /// </summary>
    /// <param name="request">An object containing pagination, sorting, and filtering parameters for retrieving the reviews.</param>
    /// <returns>A tuple containing a list of <see cref="Review"/> items and the total count of reviews that match the criteria.</returns>
    public async Task<(List<Review> Items, int TotalCount)> GetWithCountAsync(ReviewsAllRequest request) =>
        await Repository.GetAllPaginatedAsync(request);

    #endregion
}