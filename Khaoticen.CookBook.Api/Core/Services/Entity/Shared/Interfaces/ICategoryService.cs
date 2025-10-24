using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;

public interface ICategoryService
{
    #region BASE-SERVICE-implemented methods

    public Task<List<Category>> GetAllAsync();

    public Task<Category> UpdateAsync(CategoryUpdateDto dto, Category entity);

    public Task<Category?> GetAsync(Guid id);

    public Task DeleteAsync(Category entity);

    #endregion


    public Task<Category> CreateAndSaveAsync(CategoryCreateDto dto);

    public Task<Category?> GetWithRelatedAsync(Guid id);

    public Task<Category?> GetByNameAsync(string name);
}