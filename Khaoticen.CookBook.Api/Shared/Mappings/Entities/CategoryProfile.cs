using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Shared.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Shared.Mappings.Entities;

public class CategoryProfile: BaseEntityProfile<
    Category,
    CategoryCreateRequestDto,
    CategoryUpdateRequestDto,
    CategoryCreateDto,
    CategoryUpdateDto,
    CategoryResponseDto,
    CategoriesResponseDto
    >;