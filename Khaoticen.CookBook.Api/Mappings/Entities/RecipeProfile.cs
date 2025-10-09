using Khaoticen.CookBook.Api.Api.Dtos.Request.Recipe;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Recipes;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Mappings.Entities;

public class RecipeProfile : BaseEntityProfile<
    Recipe,
    RecipeCreateRequest,
    RecipeUpdateRequest,
    RecipeCreateDto,
    RecipeUpdateDto,
    RecipeResponseDto,
    RecipesResponseDto
>
{
    public RecipeProfile()
    {
        // REQUESTS => DTOs
        CreateMap<RecipeCreateRequest, RecipeCreateDto>();
        CreateMap<CategoriesUpdateRecipeRequest, RecipeCategoriesDto>();
        
        // DTOs => ENTITIES
        CreateMap<RecipeCreateDto, Recipe>()
            .ForMember(dest => dest.Categories, opt => opt.Ignore());
        
        // ENTITIES => RESPONSES
        // Reviews - include
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));

    }
}