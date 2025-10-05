using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Recipes;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories.Base;
using Khaoticen.CookBook.Api.Core.Factories.Interfaces;

namespace Khaoticen.CookBook.Api.Core.Factories;

public class RecipeFactory : BaseFactory<Recipe, RecipeCreateDto, RecipeUpdateDto>, IRecipeFactory
{
    public RecipeFactory(IMapper mapper): base(mapper)
    {
    }
    // public Recipe Create(RecipeCreateDto dto)
    // {
    //     return mapper.Map<Recipe>(dto);
    // }
    //
    // // TODO: Domain rules / overrides
    // // recipe.Title = dto.Title.Trim();          // normalization
    // // recipe.CreatedAt = DateTime.UtcNow;       // system-set field
    //
    // public void Update(RecipeUpdateDto dto, Recipe entity)
    // {
    //     mapper.Map(dto, entity);
    //
    //     // Domain rules / overrides
    //     // entity.Title = dto.Title.Trim();          // normalization
    //     // entity.CreatedAt = DateTime.UtcNow;       // system-set field
    // }
}