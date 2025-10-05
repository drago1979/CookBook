using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Create;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Update;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;

namespace Khaoticen.CookBook.Api.Core.Factories;

public class RecipeFactory : BaseFactory<Recipe, RecipeCreateDto, RecipeUpdateDto>
{
    public RecipeFactory(IMapper mapper): base(mapper)
    {
    }
}