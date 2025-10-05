using AutoMapper;
using Khaoticen.CookBook.Api.Api.DTOs;
using Khaoticen.CookBook.Api.Core.Dtos.Recipes;
using Khaoticen.CookBook.Api.Core.Entities;

namespace Khaoticen.CookBook.Api.Api.Mappings;

public class RecipeProfile: Profile // todo: prebaciti u core? Ili podeliti?
{
    public RecipeProfile()
    {
        // CRUD
        CreateMap<RecipeCreateDto, Recipe>();
        CreateMap<RecipeUpdateDto, Recipe>();
        
        // API response
        CreateMap<Recipe, RecipeResponseDto>();
    }
}