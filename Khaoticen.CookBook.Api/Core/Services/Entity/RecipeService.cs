using Khaoticen.CookBook.Api.Core.Dtos.Entity.Create;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Update;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class RecipeService : BaseEntityService<
    Recipe,
    RecipeFactory,
    RecipeCreateDto,
    RecipeUpdateDto
>
{
    private readonly ReviewService _reviewService;
    private readonly RecipeRepository _repository;

    public RecipeService(AppDbContext db, RecipeFactory factory, ReviewService reviewService,
        RecipeRepository repository)
        : base(db, factory)
    {
        _reviewService = reviewService;
        _repository = repository;
    }

    public async Task<Review> AddReview(Recipe recipe, ReviewCreateDto entityCreateDto)
    {
        var review = _reviewService.CreateForRecipe(entityCreateDto, recipe);

        await Db.SaveChangesAsync();

        var result = review;


        return review;
    }

    public override async Task<Recipe?> Get(Guid id)
    {
        return await _repository.GetByIdWithReviewsAsync(id);
    }
}