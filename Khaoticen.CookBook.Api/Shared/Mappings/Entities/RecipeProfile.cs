using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Shared.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Shared.Mappings.Entities;

public class RecipeProfile : BaseEntityProfile<
    Recipe,
    RecipeCreateRequestDto,
    RecipeUpdateRequestDto,
    RecipeCreateDto,
    RecipeUpdateDto,
    RecipeResponseDto,
    RecipeInListResponseDto
>
{
    public RecipeProfile()
    {
        // REQUESTS => DTOs
        CreateMap<RecipeCreateRequestDto, RecipeCreateDto>(); // todo!!: to base class?
        CreateMap<RecipeCategoriesUpdateRequestDto, RecipeCategoriesDto>(); // todo!!: to base class?
        
        // DTOs => ENTITIES
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore());
        
        // ENTITIES => RESPONSES
        // RELATIONSHIPS - include
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));
        
        // Entity in relationship
        CreateMap<Recipe, RecipeInRelatedResponseDto>();

    }
}