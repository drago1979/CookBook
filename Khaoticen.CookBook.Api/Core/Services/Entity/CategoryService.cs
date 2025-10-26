using AutoMapper;
using Khaoticen.CookBook.Api.Api.RequestDtos;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;
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

    public override async Task<Category> UpdateAsync(CategoryUpdateDto dto, Category entity)
    {
        EnsureNotDefaultCategory(entity);

        return await base.UpdateAsync(dto, entity);
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

    // public async Task<PaginatedResult<CategoryResponseDto>> GetPaginatedAsync(CategoriesAllRequest request)
    // {
    //     var (items, total) = await Repository.GetAllPaginatedAsync(
    //         request.Page,
    //         request.PageSize,
    //         request.SortBy,
    //         request.SortDirection,
    //         request.SearchColumn,
    //         request.SearchValue
    //     );
    //
    //     return new PaginatedResult<CategoryResponseDto>
    //     {
    //         Items = Mapper.Map<List<CategoryResponseDto>>(items),
    //         Page = request.Page,
    //         PageSize = request.PageSize,
    //         TotalCount = total
    //     };
    // }
    
    // public async Task<BasePaginatedResponse<Category>> GetWithCountAsync(CategoriesAllRequest request)
    public async Task<(List<Category> Items, int TotalCount)> GetWithCountAsync(CategoriesAllRequest request)
    {
        var (items, total) = await Repository.GetAllPaginatedAsync(
            request.Page,
            request.PageSize,
            request.SortBy,
            request.SortDirection,
            request.SearchColumn,
            request.SearchValue
        );

        return (items, total);
        
        // return new BasePaginatedResponse<Category> // todo:!!! good location?
        // {
        //     Items = items,
        //     Page = request.Page,
        //     PageSize = request.PageSize,
        //     TotalCount = total
        // };
    }

}