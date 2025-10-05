using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Base;
using Khaoticen.CookBook.Api.Api.DTOs;
using Khaoticen.CookBook.Api.Core.Dtos.Recipes;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
// public class RecipesController(IMapper mapper, IRecipeService entityService)
public class RecipesController(IMapper mapper, RecipeService entityService)
    : AppControllerBase<
        Recipe, 
        RecipeService, 
        RecipeCreateDto,
        RecipeUpdateDto,
        RecipeResponseDto
    >(entityService, mapper)
{
    #region CRUD

    // [HttpPost]
    // public async Task<ActionResult> Create([FromBody] RecipeCreateDto entityCreateDto)
    // {
    //     var entity = await entityService.Create(entityCreateDto);
    //
    //     return CreatedAtAction(
    //         nameof(GetByGuid),
    //         new { id = entity.Id },
    //         Transform(entity)
    //     );
    // }

    // [HttpGet]
    // public async Task<ActionResult> GetAll()
    // {
    //     var entities = await entityService.GetAll();
    //
    //     return Ok(Transform(entities));
    // }

    // [HttpGet("{id:guid}")]
    // public async Task<ActionResult> GetByGuid(Guid id)
    // {
    //     var entity = await entityService.Get(id);
    //
    //     if (entity == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return Ok(Transform(entity));
    // }

    // [HttpPatch("{id:guid}")]
    // public async Task<ActionResult> Patch(Guid id, [FromBody] RecipeUpdateDto entityUpdateDto,
    //     bool returnUpdated = false)
    // {
    //     // todo: u skriptu - debugging
    //     // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(entityCreateDto));
    //     var entity = await entityService.Get(id);
    //
    //     if (entity == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     await entityService.Update(entity, entityUpdateDto);
    //
    //     if (returnUpdated)
    //     {
    //         return Ok(Transform(entity));
    //     }
    //
    //     return NoContent();
    // }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id) // todo: dodaj createdAt, updatedAt u response
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        await entityService.Delete(entity);

        return NoContent();
    }

    #endregion
}