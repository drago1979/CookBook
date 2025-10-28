using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;

public interface IReviewService
{
    #region BASE-SERVICE-implemented methods

    public Task<Review> UpdateAsync(ReviewUpdateDto dto, Review entity);

    public Task<Review?> GetAsync(Guid id);
    
    public Task DeleteAsync(Review entity);

    #endregion

    public Task<Review?> GetWithRelatedAsync(Guid id);
    public Task<List<Review>> GetAllWithDeletedAsync();

    public Task<(List<Review> Items, int TotalCount)> GetWithCountAsync(ReviewsAllRequest request);
}