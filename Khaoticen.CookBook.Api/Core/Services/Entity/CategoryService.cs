using AutoMapper;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
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
    >(db, mapper, repository, factory), ICategoryService
{
    #region CRUD

    /// <summary>
    /// Creates a new category entity from the provided data transfer object (DTO) and saves it to the database.
    /// </summary>
    /// <param name="dto">The data transfer object containing the details for the new category.</param>
    /// <returns>The newly created and saved category entity.</returns>
    /// <exception cref="ValueNotAllowedException">
    /// Thrown when a category with the same name already exists.
    /// </exception>
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

    /// <summary>
    /// Retrieves a paginated and sorted list of categories along with the total count of items.
    /// </summary>
    /// <param name="request">The request containing pagination, sorting, and filtering details.</param>
    /// <returns>A tuple containing the list of categories and the total count of items.</returns>
    public async Task<(List<Category> Items, int TotalCount)> GetWithCountAsync(CategoriesAllRequest request)
    { 
        return await Repository.GetAllPaginatedAsync(request);
    }


    /// <summary>
    /// Retrieves a category with all its related data by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to retrieve.</param>
    /// <returns>Returns the category entity with all related data, or null if not found.</returns>
    public async Task<Category?> GetWithRelatedAsync(Guid id)
    {
        return await Repository.GetByIdIncludeAllRelatedAsync(id);
    }

    /// <summary>
    /// Updates a category entity using the provided update data transfer object and ensures the category is not a default category.
    /// </summary>
    /// <param name="dto">The data transfer object containing updated category data.</param>
    /// <param name="entity">The category entity to be updated.</param>
    /// <returns>The updated category entity.</returns>
    public override async Task<Category> UpdateAsync(CategoryUpdateDto dto, Category entity)
    {
        EnsureNotDefaultCategory(entity);

        return await base.UpdateAsync(dto, entity);
    }

    /// <summary>
    /// Deletes the specified category entity asynchronously. Ensures that the category being deleted is not
    /// the default category and reassigns related recipes to the default category before deletion.
    /// </summary>
    /// <param name="entity">The category entity to be deleted.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
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

    /// <summary>
    /// Retrieves a <see cref="Category"/> entity by its unique name.
    /// </summary>
    /// <param name="name">The name of the category to retrieve.</param>
    /// <returns>A task representing the asynchronous operation.
    /// The task result contains the <see cref="Category"/> entity if found; otherwise, null.</returns>
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
            throw new ValueNotAllowedException(
                $"{CategoryConstants.DefaultCategoryName} category cannot be updated or deleted.");
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