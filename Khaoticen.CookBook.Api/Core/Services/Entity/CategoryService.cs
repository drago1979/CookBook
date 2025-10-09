using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;

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
    
    public CategoryService(
        AppDbContext db, 
        CategoryFactory factory, 
        CategoryRepository repository,
        RecipeRepository recipeRepository
        )
        : base(db, factory)
    {
        _repository = repository;
        _recipeRepository = recipeRepository;
    }
    
    public override async Task<Category> CreateAndSave(CategoryCreateDto dto)
    {
        if (await _repository.GetByNameAsync(dto.Name) != null)
        {
            throw new DomainValidationException($"Category with name '{dto.Name}' already exists.");
        }

        var entity = Factory.Create(dto);

        Db.Categories.Add(entity);
        
        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task<Category?> GetByName(string name)
    {
        return await _repository.GetByNameAsync(name);
    }

    #region RELATINSHIPS // todo!! remove? or alter

    // public async Task<Review> AddRecipe(Category category, RecipeCreateDto entityCreateDto)
    // {
    //     var review = _recipeRepository.CreateForCategory(entityCreateDto, category);
    //
    //     await Db.SaveChangesAsync();
    //
    //     var result = review;
    //
    //
    //     return review;
    // }

    #endregion
}