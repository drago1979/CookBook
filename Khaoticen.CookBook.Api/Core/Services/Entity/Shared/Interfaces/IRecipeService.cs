using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;

public interface IRecipeService
{
    #region BASE-SERVICE-implemented methods

    public Task<List<Recipe>> GetAllAsync();
    public Task<Recipe?> GetAsync(Guid id);
    public Task<Recipe> UpdateAsync(RecipeUpdateDto dto, Recipe entity);

    public Task DeleteAsync(Recipe entity);

    #endregion

    public Task<Recipe> CreateAndSaveAsync(RecipeCreateDto dto);
    public Task<List<Recipe>> GetAllWithDeletedAsync();
    public Task<Recipe?> GetWithRelatedAsync(Guid id);

    public Task UpdateCategoriesAsync(Recipe recipe, RecipeCategoriesDto categories);

    public Task<Review> AddReviewAsync(Recipe recipe, ReviewCreateDto entityCreateDto);
}