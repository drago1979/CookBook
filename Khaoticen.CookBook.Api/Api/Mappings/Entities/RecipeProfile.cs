using Khaoticen.CookBook.Api.Api.Dtos.Request.Recipe;
using Khaoticen.CookBook.Api.Api.Dtos.Response;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Recipes;
using Khaoticen.CookBook.Api.Api.Mappings.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Api.Mappings.Entities;

public class RecipeProfile : BaseEntityProfile<
    Recipe,
    RecipeResponseDto,
    RecipesResponseDto,
    RecipeCreateDto,
    RecipeUpdateDto> // todo: prebaciti u core? Ili podeliti?
{
    public RecipeProfile()
    {
        CreateMap<CreateRecipeRequest, RecipeCreateDto>()
            .ForMember(dest => dest.CategoryId,
                opt => opt.MapFrom(src => Guid.Parse(src.CategoryId)));
        
        
        CreateMap<Recipe, RecipeResponseDto>()
            .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));
    }
}