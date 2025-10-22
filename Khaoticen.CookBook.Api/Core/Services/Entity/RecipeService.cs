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
    private readonly RecipeFactory _factory;
    private readonly ReviewFactory _reviewFactory;
    private readonly RecipeRepository _repository;
    private readonly CategoryRepository _categoryRepository;
    private readonly ReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public RecipeService(
        AppDbContext db,
        RecipeFactory factory,
        ReviewFactory reviewFactory,
        RecipeRepository recipeRepository,
        CategoryRepository categoryRepository,
        ReviewRepository reviewRepository,
        IMapper mapper
    )
        : base(db, factory)
    {
        _factory = factory;
        _reviewFactory = reviewFactory;
        _repository = recipeRepository;
        _categoryRepository = categoryRepository;
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    #region CRUD

    public async Task<Recipe> CreateAndSaveAsync(RecipeCreateDto dto)
    {
        var (nonExistingIds, idsToAdd) = await CategoryIdsStatusWhenCreateAsync(dto.Categories);

        EnsureCategoriesExist(nonExistingIds);

        var entity = Factory.Create(dto);

        await SyncRecipeCategories(entity, idsToAdd); // todo: async?

        _repository.Add(entity);

        EnsureRecipeHasCategory(entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task<Recipe?> GetAsync(Guid id)
    {
        return await _repository.GetByIdIncludeAllRelatedAsync(id);
    }

    public async Task<List<Recipe>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateAsync(RecipeUpdateDto dto, Recipe entity)
    {
        _mapper.Map(dto, entity);

        _repository.Update(entity);

        await Db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Recipe entity)
    {
        entity.SoftDelete();
        
        _repository.Update(entity);

        await Db.SaveChangesAsync();
    }

    #endregion


    #region RELATIONSHIPS

    public async Task UpdateCategoriesAsync(Recipe recipe, RecipeCategoriesDto categories)
    {
        var (nonExisting, toAdd, toRemove) = await CategoryIdsStatusWhenUpdateAsync(categories, recipe);

        EnsureCategoriesExist(nonExisting);

        await SyncRecipeCategories(recipe, toAdd, toRemove);

        EnsureRecipeHasCategory(recipe);

        await Db.SaveChangesAsync();
    }

    public async Task<Review> AddReviewAsync(Recipe recipe, ReviewCreateDto entityCreateDto)
    {
        var review = _reviewFactory.CreateForRecipe(entityCreateDto, recipe);

        await Db.SaveChangesAsync();

        var result = review;


        return review;
    }

    #endregion

    #region VALIDATION

    private void EnsureRecipeHasCategory(Recipe recipe)
    {
        if (recipe.Categories.Count == 0)
            throw new InvalidRecipeException("Recipe must have at least one category.");
    }

    private void EnsureCategoriesExist(List<Guid> nonExisting)
    {
        if (nonExisting.Count == 0) return;

        var ids = string.Join(", ", nonExisting);
        throw new ValueNotAllowedException($"Following categories do not exist: {ids}");
    }

    #endregion

    #region HELPERS

    private async Task<(List<Guid> nonExisting, List<Guid> toAdd, List<Guid> toRemove)>
        CategoryIdsStatusWhenUpdateAsync(RecipeCategoriesDto dto, Recipe recipe)
    {
        var submittedIds = dto.CategoryIds.Distinct().ToList();

        var existingIds = await _categoryRepository.GetExistingIdsAsync(submittedIds);
        var nonExisting = submittedIds.Except(existingIds).ToList();

        var currentlyOwns = await _repository.GetOwnCategoryIdsAsync(recipe);
        var dontTouch = existingIds.Intersect(currentlyOwns).ToList();

        var toAdd = existingIds.Except(dontTouch).ToList();
        var toRemove = currentlyOwns.Except(dontTouch).ToList();

        return (nonExisting, toAdd, toRemove);
    }

    private async Task<(List<Guid> nonExisting, List<Guid> toAdd)> CategoryIdsStatusWhenCreateAsync(
        RecipeCategoriesDto dto)
    {
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