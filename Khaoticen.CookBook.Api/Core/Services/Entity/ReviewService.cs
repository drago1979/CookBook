using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class ReviewService : BaseEntityService<
    Review,
    Factories.ReviewFactory,
    ReviewCreateDto,
    ReviewUpdateDto
>
{
    public ReviewService(AppDbContext db, Factories.ReviewFactory factory)
        : base(db, factory)
    {
    }
}