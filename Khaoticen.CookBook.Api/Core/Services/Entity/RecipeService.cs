using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class RecipeService : BaseEntityService<
    Recipe,
    RecipeFactory,
    RecipeCreateDto,
    RecipeUpdateDto
>
{
    private readonly RecipeFactory _recipeFactory;
    private readonly ReviewFactory _reviewFactory;
    private readonly RecipeRepository _recipeRepository;
    private readonly CategoryRepository _categoryRepository;

    public RecipeService(
        AppDbContext db,
        RecipeFactory factory,
        ReviewFactory reviewFactory,
        RecipeRepository recipeRepository,
        CategoryRepository categoryRepository
    )
        : base(db, factory)
    {
        _recipeFactory = factory;
        _reviewFactory = reviewFactory;
        _recipeRepository = recipeRepository;
        _categoryRepository = categoryRepository;
    }


    public override async Task<Recipe> CreateAndSave(RecipeCreateDto dto)
    {
        // if(!Guid.TryParse(dto.CategoryId, out var categoryId))
        //     throw new DomainValidationException("bad");
        
        var category = await _categoryRepository.GetById(dto.CategoryId);
        
        // var category = await _categoryRepository.GetById(categoryId);
        
        // var category = await _categoryRepository.GetById(dto.CategoryId);

        if (category == null)
        {
            throw new DomainValidationException($"Category with ID '{dto.CategoryId}' does not exist.");
        }

        var recipe =
            _recipeFactory.CreateForCategory(dto, category); // todo: create factory method to enforce relationship?

        recipe.Categories.Add(category!);

        Db.Recipes.Add(recipe);

        await Db.SaveChangesAsync();

        return recipe;
    }


    public override async Task<Recipe?> Get(Guid id)
    {
        return await _recipeRepository.GetByIdWithReviewsAsync(id);
    }

    #region RELATIONSHIPS

    public async Task<Review> AddReview(Recipe recipe, ReviewCreateDto entityCreateDto)
    {
        var review = _reviewFactory.CreateForRecipe(entityCreateDto, recipe);

        await Db.SaveChangesAsync();

        var result = review;


        return review;
    }

    #endregion
}