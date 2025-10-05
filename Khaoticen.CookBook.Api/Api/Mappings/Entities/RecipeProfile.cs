using Khaoticen.CookBook.Api.Api.Dtos.Response;
using Khaoticen.CookBook.Api.Api.Mappings.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Create;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Update;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Api.Mappings.Entities;

public class RecipeProfile: BaseEntityProfile<Recipe, RecipeResponseDto, RecipeCreateDto, RecipeUpdateDto> // todo: prebaciti u core? Ili podeliti?
{
    public RecipeProfile()
    {
        // todo: customize logic
    }
}