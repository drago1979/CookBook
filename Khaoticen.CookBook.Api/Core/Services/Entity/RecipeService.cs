using AutoMapper;
using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class RecipeService(
    AppDbContext db,
    RecipeFactory factory,
    ReviewFactory reviewFactory,
    RecipeRepository repository,
    CategoryRepository categoryRepository,
    IMapper mapper
)
    : BaseEntityService<
        Recipe,
        RecipeRepository,
        RecipeFactory,
        RecipeCreateDto,
        RecipeUpdateDto
    >(db, mapper, repository, factory), IRecipeService
{
    #region CRUD

    public async Task<Recipe> CreateAndSaveAsync(RecipeCreateDto dto)
    {
        var (nonExistingIds, idsToAdd) = await CategoryIdsStatusWhenCreateAsync(dto.Categories);

        EnsureCategoriesExist(nonExistingIds);

        var entity = Factory.Create(dto);

        await SyncRecipeCategories(entity, idsToAdd);

        Repository.Add(entity);

        EnsureRecipeHasCategory(entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task<(List<Recipe> Items, int TotalCount)> GetWithCountAsync(RecipesAllRequest request,
        bool withDeleted = false)
    {
        if (request.CategoryId is { } categoryId) await EnsureCategoryExistsAsync(categoryId);
        
        var (items, total) = await Repository.GetAllPaginatedAsync(request, withDeleted);
        
        return (items, total);
    }

    public async Task<Recipe?> GetWithRelatedAsync(Guid id)
    {
        return await Repository.GetByIdIncludeAllRelatedAsync(id);
    }

    public override async Task DeleteAsync(Recipe entity)
    {
        await Repository.GetByIdIncludeAllRelatedAsync(entity.Id);

        entity.SoftDelete();

        Repository.Update(entity);

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
        var review = reviewFactory.CreateForRecipe(entityCreateDto, recipe);

        await Db.SaveChangesAsync();

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

    private async Task EnsureCategoryExistsAsync(Guid categoryId)
    {
        if (await categoryRepository.GetByIdAsync(categoryId) is null)
            throw new EntityNotFoundException($"Category ID: {categoryId} not found");
    }

    #endregion

    #region HELPERS

    private async Task<(List<Guid> nonExisting, List<Guid> toAdd, List<Guid> toRemove)>
        CategoryIdsStatusWhenUpdateAsync(RecipeCategoriesDto dto, Recipe recipe)
    {
        var submittedIds = dto.CategoryIds.Distinct().ToList();

        var existingIds = await categoryRepository.GetExistingIdsAsync(submittedIds);
        var nonExisting = submittedIds.Except(existingIds).ToList();

        var currentlyOwns = await GetOwnCategoryIdsAsync(recipe);
        var dontTouch = existingIds.Intersect(currentlyOwns).ToList();

        var toAdd = existingIds.Except(dontTouch).ToList();
        var toRemove = currentlyOwns.Except(dontTouch).ToList();

        return (nonExisting, toAdd, toRemove);
    }

    private async Task<(List<Guid> nonExisting, List<Guid> toAdd)> CategoryIdsStatusWhenCreateAsync(
        RecipeCategoriesDto dto)
    {
        var categoryIds = dto.CategoryIds.Distinct().ToList();
        var toAdd = await categoryRepository.GetExistingIdsAsync(categoryIds);
        var nonExisting = categoryIds.Except(toAdd).ToList();

        return (nonExisting, toAdd);
    }

    private async Task SyncRecipeCategories(Recipe recipe, List<Guid> toAdd, List<Guid>? toRemove = null)
    {
        var categoriesToAdd = await categoryRepository
            .GetAllByIdsAsync(toAdd);

        foreach (var category in categoriesToAdd)
        {
            recipe.Categories.Add(category);
        }

        if (toRemove != null)
        {
            var categoriesToRemove = await categoryRepository
                .GetAllByIdsAsync(toRemove);

            foreach (var category in categoriesToRemove)
            {
                recipe.Categories.Remove(category);
            }
        }
    }

    private async Task<List<Guid>> GetOwnCategoryIdsAsync(Recipe recipe)
    {
        var recipeWithRelations = await Repository.GetByIdIncludeAllRelatedAsync(recipe.Id);

        return recipeWithRelations?.Categories.Select(c => c.Id).ToList() ?? [];
    }

    #endregion
}