using Khaoticen.CookBook.Api.Api.Dtos.Response.Categories;
using Khaoticen.CookBook.Api.Api.Mappings.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Api.Mappings.Entities;

public class CategoryProfile : BaseEntityProfile<
    Category,
    CategoryResponseDto,
    CategoriesResponseDto,
    CategoryCreateDto,
    CategoryUpdateDto>;