using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
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

    public CategoryService(AppDbContext db, CategoryFactory factory, CategoryRepository repository)
        : base(db, factory)
    {
        _repository = repository;
    }

    public override async Task<Category> CreateAndSave(CategoryCreateDto dto)
    {
        if (await _repository.GetByNameAsync(dto.Name) != null)
        {
            throw new ValidationException("Category already exists");
        }
        
        var entity = Factory.Create(dto);

        Db.Categories.Add(entity);

        return entity;
    }
}