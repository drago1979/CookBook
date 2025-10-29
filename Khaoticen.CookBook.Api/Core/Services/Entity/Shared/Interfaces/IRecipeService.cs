using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;

public interface IRecipeService
{
    #region BASE-SERVICE-implemented methods
    
    public Task<Recipe?> GetAsync(Guid id);
    public Task<Recipe> UpdateAsync(RecipeUpdateDto dto, Recipe entity);

    public Task DeleteAsync(Recipe entity);

    #endregion

    public Task<Recipe> CreateAndSaveAsync(RecipeCreateDto dto);
    
    public Task<(List<Recipe> Items, int TotalCount)> GetWithCountAsync(RecipesAllRequest request, bool withDeleted = false);
    
    public Task<Recipe?> GetWithRelatedAsync(Guid id, bool withDeleted = false);

    public Task UpdateCategoriesAsync(Recipe recipe, RecipeCategoriesDto categories);

    public Task<Review> AddReviewAsync(Recipe recipe, ReviewCreateDto entityCreateDto);
}