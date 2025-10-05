using Khaoticen.CookBook.Api.Core.Dtos.Recipes;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;

public interface IRecipeService
{
    public Task<Recipe> Create(RecipeCreateDto dto);
    public Task<Recipe?> Get(Guid id);
    public Task<List<Recipe>> GetAll();
    public Task<Recipe> Update(Recipe entity, RecipeUpdateDto dto);
    public Task Delete(Recipe entity);
    
}