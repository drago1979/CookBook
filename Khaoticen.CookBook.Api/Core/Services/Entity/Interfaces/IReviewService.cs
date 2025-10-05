using Khaoticen.CookBook.Api.Core.Dtos.Reviews;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;

public interface IReviewService
{
    public Task<Review> Create(ReviewCreateDto dto);
     public Task<Review?> Get(Guid id);
//     public Task<List<Review>> GetAll();
//     public Task<Review> Update(Review entity, ReviewUpdateDto dto);
//     public Task Delete(Review entity);
}