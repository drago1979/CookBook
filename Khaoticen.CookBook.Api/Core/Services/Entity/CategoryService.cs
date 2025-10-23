using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Constants;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class CategoryService(
    AppDbContext db,
    CategoryFactory factory,
    CategoryRepository repository,
    IMapper mapper
)
    : BaseEntityService<
        Category,
        CategoryRepository,
        CategoryFactory,
        CategoryCreateDto,
        CategoryUpdateDto
    >(db, mapper, repository, factory)
{
    #region CRUD

    public async Task<Category> CreateAndSaveAsync(CategoryCreateDto dto)
    {
        if (await Repository.GetByNameAsync(dto.Name) != null)
        {
            throw new ValueNotAllowedException($"Category with name '{dto.Name}' already exists.");
        }

        var entity = Factory.Create(dto);

        Repository.Add(entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task<Category?> GetWithRelatedAsync(Guid id)
    {
        return await Repository.GetByIdIncludeAllRelatedAsync(id);
    }
    
    public override async Task DeleteAsync(Category entity)
    {
        var defaultCategory = await GetDefaultCategoryOrThrowAsync();

        EnsureNotDefaultCategory(entity);

        var entityWithRelations = await GetWithRelationsOrThrowAsync(entity);

        ReassignRecipes(entityWithRelations, defaultCategory);

        Repository.Delete(entityWithRelations);

        await Db.SaveChangesAsync();
    }

    #endregion

    #region MISC

    public async Task<Category?> GetByNameAsync(string name) =>
        await Repository.GetByNameAsync(name);

    #endregion

    #region HELPERS

    private async Task<Category> GetDefaultCategoryOrThrowAsync() =>
        await Repository.GetByIdAsync(CategoryConstants.DefaultCategoryId) ??
        throw new EntityNotFoundException($"{CategoryConstants.DefaultCategoryName} category not found.");

    private async Task<Category> GetWithRelationsOrThrowAsync(Category entity) =>
        await Repository.GetByIdIncludeAllRelatedAsync(entity.Id) ??
        throw new EntityNotFoundException($"Category ID: {entity.Id} not found");

    private void EnsureNotDefaultCategory(Category entity)
    {
        if (entity.Id == CategoryConstants.DefaultCategoryId)
            throw new ValueNotAllowedException($"{CategoryConstants.DefaultCategoryName} category cannot be deleted.");
    }

    private void ReassignRecipes(Category entity, Category defaultCategory)
    {
        foreach (var recipe in entity.Recipes.ToList())
        {
            if (recipe.Categories.Count == 1)
            {
                recipe.Categories.Remove(entity);
                recipe.Categories.Add(defaultCategory);
            }
            else
            {
                recipe.Categories.Remove(entity);
            }
        }
    }

    #endregion
}