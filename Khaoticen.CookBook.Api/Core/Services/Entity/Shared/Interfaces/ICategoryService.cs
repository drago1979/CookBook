using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;

public interface ICategoryService
{
    #region BASE-SERVICE-implemented methods

    public Task<Category> UpdateAsync(CategoryUpdateDto dto, Category entity);

    public Task DeleteAsync(Category entity);

    #endregion


    public Task<Category> CreateAndSaveAsync(CategoryCreateDto dto);

    public Task<Category?> GetWithRelatedAsync(Guid id);

    public Task<Category?> GetByNameAsync(string name);

    public Task<(List<Category> Items, int TotalCount)> GetWithCountAsync(CategoriesAllRequest request);
}