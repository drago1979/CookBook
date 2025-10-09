using System.Security.Cryptography.Xml;
using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.Dtos.Request.Recipe;
using Khaoticen.CookBook.Api.Api.Dtos.Request.Review;
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
>(mapper) // todo!!
{
    #region CRUD

    /// <summary>
    /// Accepts multiple Categories
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost(Name = "RecipeCreate")]
    public async Task<ActionResult> Create([FromBody] RecipeCreateRequest request)
    {
        var createDto = TransformToCreateDto(request);

        var entity = await entityService.CreateAndSave(createDto);

        return CreatedAtAction(
            nameof(GetByGuid),
            new { id = entity.Id },
            TransformEntityToResponse(entity)
        );
    }

    [HttpGet(Name = "RecipeGetAll")]
    public async Task<ActionResult> GetAll()
    {
        var entities = await entityService.GetAll();

        return Ok(TransformEntitiesToResponse(entities));
    }

    [HttpGet("{id:guid}", Name = "RecipeGetByGuid")]
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }

    [HttpPatch("{id:guid}", Name = "RecipePatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] RecipeUpdateRequest request,
        bool returnUpdated = false)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateEntityDto = TransformToUpdateDto(request);

        await entityService.Update(entity, updateEntityDto);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
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

    [HttpPut("{id:guid}/categories", Name = "RecipeUpdateCategories")]
    public async Task<ActionResult> UpdateCategories(Guid id, [FromBody] CategoriesUpdateRecipeRequest request,
        bool returnUpdated = false)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateCategoriesDto = TransformToRecipeCategoriesCreateDto(request);

        await entityService.UpdateCategories(entity, updateCategoriesDto);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }


    [HttpPost("{id:guid}/reviews", Name = "RecipeAddReview")]
    public async Task<ActionResult> AddReview(Guid id, [FromBody] ReviewCreateRequest createRequest)
    {
        var recipe = await entityService.Get(id);

        if (recipe == null)
        {
            return NotFound();
        }

        var createDto = mapper.Map<ReviewCreateDto>(createRequest);

        var review = await entityService.AddReview(recipe, createDto);

        return CreatedAtRoute(
            routeName: "ReviewGetByGuid",
            routeValues: new { id = review.Id },
            value: Transform(review)
        );
    }

    #endregion

    #region HELPERS

    private RecipeCreateDto TransformToCreateDto(RecipeCreateRequest request)
    {
        return mapper.Map<RecipeCreateDto>(request);
    }

    private RecipeUpdateDto TransformToUpdateDto(RecipeUpdateRequest request)
    {
        return mapper.Map<RecipeUpdateDto>(request);
    }

    public RecipeCategoriesDto TransformToRecipeCategoriesCreateDto(CategoriesUpdateRecipeRequest request)
    {
        return mapper.Map<RecipeCategoriesDto>(request);
    }

    private ReviewResponseDto Transform(Review entity)
    {
        return Mapper.Map<ReviewResponseDto>(entity);
    }

    #endregion
}