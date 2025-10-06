using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class ReviewService : BaseEntityService<
    Review,
    ReviewFactory,
    ReviewCreateDto,
    ReviewUpdateDto
>
{
    public ReviewService(AppDbContext db, ReviewFactory factory)
        : base(db, factory)
    {
    }

    public Review CreateForRecipe(ReviewCreateDto dto, Recipe recipe)
    {
        var review = Create(dto);

        review.RecipeId = recipe.Id;
        review.Recipe = recipe;
        recipe.Reviews.Add(review);

        return review;
    }
}