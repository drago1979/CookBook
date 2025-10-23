using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;

namespace Khaoticen.CookBook.Api.Core.Factories;

public class ReviewFactory(IMapper mapper) : BaseFactory<Review, ReviewCreateDto>(mapper)
{
    public Review CreateForRecipe(ReviewCreateDto dto, Recipe recipe)
    {
        var review = Create(dto);

        recipe.Reviews.Add(review);

        return review;
    }
}