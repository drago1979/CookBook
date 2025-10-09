using Khaoticen.CookBook.Api.Api.Dtos.Request.Category;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Categories;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Mappings.Entities;

public class CategoryProfile: BaseEntityProfile<
    Category,
    CategoryCreateRequest,
    CategoryUpdateRequest,
    CategoryCreateDto,
    CategoryUpdateDto,
    CategoryResponseDto,
    CategoriesResponseDto
    >;