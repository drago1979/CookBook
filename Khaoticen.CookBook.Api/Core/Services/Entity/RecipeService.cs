using Khaoticen.CookBook.Api.Core.Dtos.Entity.Create;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Update;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Khaoticen.CookBook.Api.Infrastructure;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class RecipeService :
    BaseEntityService<
        Recipe,
        RecipeFactory,
        RecipeCreateDto,
        RecipeUpdateDto
    >
    ,
    IEntityService<
        Recipe,
        RecipeCreateDto,
        RecipeUpdateDto
    >
{
    private readonly ReviewService _reviewService;

    public RecipeService(AppDbContext db, RecipeFactory factory, ReviewService reviewService)
        : base(db, factory)
    {
        _reviewService = reviewService;
    }

    public async Task<Review> AddReview(Recipe recipe, ReviewCreateDto entityCreateDto)
    {
        var review = _reviewService.CreateForRecipe(entityCreateDto, recipe);
    
        await Db.SaveChangesAsync();
        
        var result = review;
        
    
        return review;
    }

    // public Task<Recipe> Create(RecipeCreateDto dto)
    // {
    //     throw new NotImplementedException();
    // }
}