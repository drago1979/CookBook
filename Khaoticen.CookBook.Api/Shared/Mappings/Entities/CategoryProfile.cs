using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Shared.Mappings.Entities.Shared;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Shared.Mappings.Entities;

public class CategoryProfile : BaseEntityProfile<
    Category,
    CategoryCreateRequestDto,
    CategoryUpdateRequestDto,
    CategoryCreateDto,
    CategoryUpdateDto,
    CategoryResponseDto,
    CategoriesResponseDto
>
{
    public CategoryProfile()
    {
        // ENTITIES => RESPONSES
        // RELATIONSHIPS - include
        CreateMap<Category, CategoryResponseDto>()
            .ForMember(dest => dest.Recipes, opt => opt.MapFrom(src => src.Recipes));
        
        // Entity in relationship
        CreateMap<Category, CategoryInRelatedResponseDto>();
    }
}