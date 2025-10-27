using System.Reflection;
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

    public async Task<Review?> GetWithRelatedAsync(Guid id)
    {
        return await Repository.GetByIdIncludeAllRelatedAsync(id);
    }
    
    public async Task<List<Review>> GetAllWithDeletedAsync() =>
        await Repository.GetAllWithDeletedAsync();
    
    public Task<(List<Review> Items, int TotalCount)> GetWithCountAsync(ReviewsAllRequest request)
        => base.GetWithCountAsync(request);

    #endregion
}