using System.Security.Cryptography.Xml;
using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.Dtos.Response;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Recipes;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Reviews;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController(RecipeService entityService, IMapper mapper) : AppControllerBase<
    Recipe, 
    RecipeResponseDto,
    RecipesResponseDto
>(mapper)
{
    #region  CRUD

    [HttpPost(Name = "RecipeCreate")]
    public async Task<ActionResult> Create([FromBody] RecipeCreateDto entityCreateDto)
    {
        var entity = await entityService.CreateAsync(entityCreateDto);

        return CreatedAtAction(
            nameof(GetByGuid),
            new { id = entity.Id },
            TransformEntity(entity)
        );
    }
    
    [HttpGet("{id:guid}", Name = "RecipeGetByGuid")]
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntity(entity));
    }
    
    [HttpGet(Name = "RecipeGetAll")]
    public async Task<ActionResult> GetAll()
    {
        var entities = await entityService.GetAll();

        return Ok(TransformEntities(entities));
    }
    
    [HttpPatch("{id:guid}", Name = "RecipePatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] RecipeUpdateDto entityUpdateDto,
        bool returnUpdated = false)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        await entityService.Update(entity, entityUpdateDto);

        if (returnUpdated)
        {
            return Ok(TransformEntity(entity));
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "RecipeDelete")]
    public async Task<ActionResult> Delete(Guid id)
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

    #region RELATIONSHIPS

    [HttpPost("{id:guid}/reviews", Name = "RecipeAddReview")]
    public async Task<ActionResult> AddReview(Guid id, [FromBody] ReviewCreateDto entityCreateDto)
    {
        var recipe = await entityService.Get(id);
        
        if (recipe == null)
        {
            return NotFound();
        }
        
        var review = await entityService.AddReview(recipe, entityCreateDto);
       
        return CreatedAtRoute(
            routeName: "ReviewGetByGuid",
            routeValues: new { id = review.Id },
            value: Transform(review)
        );
    }
    
    #endregion

    #region HELPERS

    private ReviewResponseDto Transform(Review entity)
    {
        return Mapper.Map<ReviewResponseDto>(entity);
    }

    #endregion

}