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

    /// <summary>
    /// Creates a new recipe entity based on the provided data transfer object (DTO) and saves it to the database.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details required to create a new recipe.</param>
    /// <returns>The created and saved recipe entity.</returns>
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

    /// <summary>
    /// Retrieves a list of recipes along with the total count, based on the specified request parameters.
    /// </summary>
    /// <param name="request">The request parameters for retrieving and filtering recipes, including pagination, sorting, and optional category filtering.</param>
    /// <param name="withDeleted">Specifies whether to include deleted recipes in the results. Defaults to false.</param>
    /// <returns>A tuple containing a list of recipes and the total count of matching recipes.</returns>
    public async Task<(List<Recipe> Items, int TotalCount)> GetWithCountAsync(RecipesAllRequest request,
        bool withDeleted = false)
    {
        if (request.CategoryId is { } categoryId) await EnsureCategoryExistsAsync(categoryId);
        
        var (items, total) = await Repository.GetAllPaginatedAsync(request, withDeleted);
        
        return (items, total);
    }

    /// <summary>
    /// Retrieves a single recipe entity by its identifier, including all related data.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <param name="withDeleted">A boolean flag indicating whether to include soft-deleted entities in the result.</param>
    /// <returns>The requested recipe entity with all related data, or null if not found.</returns>
    public async Task<Recipe?> GetWithRelatedAsync(Guid id, bool withDeleted = false)
    {
        return await Repository.GetByIdIncludeAllRelatedAsync(id, withDeleted);
    }

    /// <summary>
    /// Marks the specified recipe entity as soft deleted and updates it in the database.
    /// </summary>
    /// <param name="entity">The recipe entity to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override async Task DeleteAsync(Recipe entity)
    {
        await Repository.GetByIdIncludeAllRelatedAsync(entity.Id);

        entity.SoftDelete();

        Repository.Update(entity);

        await Db.SaveChangesAsync();
    }

    #endregion

    #region RELATIONSHIPS

    /// Updates the categories associated with a recipe.
    /// <param name="recipe">
    /// The recipe entity whose categories are to be updated.
    /// </param>
    /// <param name="categories">
    /// The data transfer object containing the updated list of category IDs.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// </returns>
    public async Task UpdateCategoriesAsync(Recipe recipe, RecipeCategoriesDto categories)
    {
        var (nonExisting, toAdd, toRemove) = await CategoryIdsStatusWhenUpdateAsync(categories, recipe);

        EnsureCategoriesExist(nonExisting);

        await SyncRecipeCategories(recipe, toAdd, toRemove);

        EnsureRecipeHasCategory(recipe);

        await Db.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a new review to the specified recipe and saves it in the database.
    /// </summary>
    /// <param name="recipe">The recipe to which the review will be added.</param>
    /// <param name="entityCreateDto">The data transfer object containing the review details to be created.</param>
    /// <returns>Returns the newly created review.</returns>
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
            throw new InvalidRecipeException();
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