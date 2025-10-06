using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;

namespace Khaoticen.CookBook.Api.Core.Factories;

public class RecipeFactory : BaseFactory<Recipe, RecipeCreateDto, RecipeUpdateDto>
{
    public RecipeFactory(IMapper mapper): base(mapper) // todo: primary constr?
    {
    }
}