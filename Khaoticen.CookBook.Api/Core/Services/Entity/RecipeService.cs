using Khaoticen.CookBook.Api.Core.Dtos.Recipes;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Factories.Interfaces;
using Khaoticen.CookBook.Api.Core.Services.Entity.Base;
using Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;
using Khaoticen.CookBook.Api.Infrastructure;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

// public class RecipeService : BaseEntityService<Recipe, RecipeFactory, RecipeCreateDto, RecipeUpdateDto>,
// IEntityService<Recipe, RecipeCreateDto, RecipeUpdateDto>
// {
//     public RecipeService(AppDbContext db, RecipeFactory factory)
//         : base(db, factory)
//     {
//     }
 
// ### IF BASED
public class RecipeService : BaseEntityService<Recipe, RecipeFactory, RecipeCreateDto, RecipeUpdateDto>,
    IEntityService<Recipe, RecipeCreateDto, RecipeUpdateDto>
{
    public RecipeService(AppDbContext db, IRecipeFactory factory)
        : base(db, factory)
    {
    }

    // public async Task<Recipe> Create(RecipeCreateDto dto)
    // {
    //     var recipe = factory.Create(dto);
    //
    //     db.Recipes.Add(recipe);
    //     await db.SaveChangesAsync();
    //
    //     return recipe;
    // }
    //
    // public async Task<Recipe?> Get(Guid id)
    // {
    //     return await db.Recipes.FindAsync(id);
    // }

    // public async Task<List<Recipe>> GetAll()
    // {
    //     return await db.Recipes.ToListAsync();
    // }
    //
    // public async Task<Recipe> Update(Recipe entity, RecipeUpdateDto dto)
    // {
    //     factory.Update(dto, entity);
    //     // db.Recipes.Update(entity);
    //     await db.SaveChangesAsync();
    //
    //     return entity;
    // }
    //
    // public async Task Delete(Recipe entity)
    // {
    //     db.Recipes.Remove(entity);
    //
    //     await db.SaveChangesAsync();
    // }


}