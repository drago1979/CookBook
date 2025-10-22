using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;
using Khaoticen.CookBook.Api.Shared.Constants;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class CategoryService : BaseEntityService<
    Category,
    CategoryFactory,
    CategoryCreateDto,
    CategoryUpdateDto
>
{
    private readonly CategoryRepository _repository;
    private RecipeRepository _recipeRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        AppDbContext db,
        CategoryFactory factory,
        CategoryRepository repository,
        RecipeRepository recipeRepository,
        IMapper mapper
    )
        : base(db, factory)
    {
        _repository = repository;
        _recipeRepository = recipeRepository;
        _mapper = mapper;
    }

    #region CRUD

    public async Task<Category> CreateAndSaveAsync(CategoryCreateDto dto)
    {
        if (await _repository.GetByNameAsync(dto.Name) != null)
        {
            throw new ValueNotAllowedException($"Category with name '{dto.Name}' already exists.");
        }

        var entity = Factory.Create(dto);

        _repository.Add(entity);

        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task<Category?> GetAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateAsync(CategoryUpdateDto dto, Category entity)
    {
        _mapper.Map(dto, entity);

        _repository.Update(entity);

        await Db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category entity)
    {
        var defaultCategory = await GetDefaultCategoryOrThrowAsync();

        EnsureNotDefaultCategory(entity);

        var entityWithRelations = await _repository.GetByIdWithRecipesAndCategoriesAsync(entity.Id) ?? 
                                  throw new EntityNotFoundException($"Category ID: {entity.Id} not found");

        ReassignRecipes(entityWithRelations, defaultCategory!);

        var x = entityWithRelations;
        
        _repository.Delete(entityWithRelations);

        await Db.SaveChangesAsync();
    }

    #endregion

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _repository.GetByNameAsync(name);
    }

    #region HELPERS

    private async Task<Category?> GetDefaultCategoryOrThrowAsync() =>
        await _repository.GetByIdAsync(CategoryConstants.DefaultCategoryId) ??
        throw new EntityNotFoundException($"{CategoryConstants.DefaultCategoryName} category not found.");

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
                // _recipeRepository.Update(recipe);
            }
            else
            {
                recipe.Categories.Remove(entity);
            }
            
        }


        var recipes = entity.Recipes.ToList();

        var x = entity;
    }

    #endregion


}