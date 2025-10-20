using AutoMapper;
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
    private readonly IMapper _mapper;

    public RecipeService(
        AppDbContext db,
        RecipeFactory factory,
        ReviewFactory reviewFactory,
        RecipeRepository recipeRepository,
        CategoryRepository categoryRepository,
        IMapper mapper
    )
        : base(db, factory)
    {
        _recipeFactory = factory;
        _reviewFactory = reviewFactory;
        _recipeRepository = recipeRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    #region CRUD

    public override async Task<Recipe> CreateAndSave(RecipeCreateDto createDto)
    {
        var (nonExisting, toAdd) = await CategoryIdsStatusWhenCreateAsync(createDto.Categories);

        ValidateCategories(nonExisting);

        var recipe = _mapper.Map<Recipe>(createDto);

        await SyncRecipeCategories(recipe, toAdd);

        Db.Recipes.Add(recipe); // todo!! ??? i sledece posle add?

        ValidateRecipe(recipe);

        await Db.SaveChangesAsync();

        return recipe;
    }

    public override async Task<Recipe?> Get(Guid id)
    {
        return await _recipeRepository.GetByIdIncludeAllRelatedAsync(id);
    }

    #endregion


    #region RELATIONSHIPS

    public async Task UpdateCategories(Recipe recipe, RecipeCategoriesDto categories)
    {
        var (nonExisting, toAdd, toRemove) = await CategoryIdsStatusWhenUpdateAsync(categories, recipe);

        ValidateCategories(nonExisting);

        await SyncRecipeCategories(recipe, toAdd, toRemove);

        ValidateRecipe(recipe);

        await Db.SaveChangesAsync();
    }

    public async Task<Review> AddReview(Recipe recipe, ReviewCreateDto entityCreateDto)
    {
        var review = _reviewFactory.CreateForRecipe(entityCreateDto, recipe);

        await Db.SaveChangesAsync();

        var result = review;


        return review;
    }

    #endregion

    #region VALIDATION

    private void ValidateRecipe(Recipe recipe)
    {
        if (recipe.Categories.Count == 0)
            throw new DomainValidationException("Recipe must have at least one category.");
    }

    private void ValidateCategories(List<Guid> nonExisting)
    {
        if (nonExisting.Count == 0) return;

        var ids = string.Join(", ", nonExisting);
        throw new DomainValidationException($"Following categories do not exist: {ids}");
    }

    #endregion

    #region HELPERS

    private async Task<(List<Guid> nonExisting, List<Guid> toAdd, List<Guid> toRemove)>
        CategoryIdsStatusWhenUpdateAsync(RecipeCategoriesDto dto, Recipe recipe)
    {
        var submittedIds = dto.CategoryIds.Distinct().ToList();

        var existingIds = await _categoryRepository.GetExistingIdsAsync(submittedIds);
        var nonExisting = submittedIds.Except(existingIds).ToList();

        var currentlyOwns = await _recipeRepository.GetOwnCategoryIdsAsync(recipe);
        var dontTouch = existingIds.Intersect(currentlyOwns).ToList();

        var toAdd = existingIds.Except(dontTouch).ToList();
        var toRemove = currentlyOwns.Except(dontTouch).ToList();

        return (nonExisting, toAdd, toRemove);
    }

    private async Task<(List<Guid> nonExisting, List<Guid> toAdd)> CategoryIdsStatusWhenCreateAsync(
        RecipeCategoriesDto dto)
    {
        var x = dto;
        
        // var toAdd = await _categoryRepository.GetExistingIdsAsync(dto.CategoryIds);
        var categoryIds = dto.CategoryIds.Distinct().ToList();

        var toAdd = await _categoryRepository.GetExistingIdsAsync(categoryIds);
        var nonExisting = categoryIds.Except(toAdd).ToList();

        return (nonExisting, toAdd);
    }

    private async Task SyncRecipeCategories(Recipe recipe, List<Guid> toAdd, List<Guid>? toRemove = null)
    {
        var categoriesToAdd = await _categoryRepository
            .GetAllByIdsAsync(toAdd);

        foreach (var category in categoriesToAdd)
        {
            recipe.Categories.Add(category);
        }

        if (toRemove != null)
        {
            var categoriesToRemove = await _categoryRepository
                .GetAllByIdsAsync(toRemove);

            foreach (var category in categoriesToRemove)
            {
                recipe.Categories.Remove(category);
            }
        }
    }

    #endregion
}