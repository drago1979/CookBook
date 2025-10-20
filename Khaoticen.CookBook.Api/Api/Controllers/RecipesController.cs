using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
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
    /// <param name="requestDto"></param>
    /// <returns></returns>
    [HttpPost(Name = "RecipeCreate")]
    public async Task<ActionResult> Create([FromBody] RecipeCreateRequestDto requestDto)
    {
        var createDto = TransformToCreateDto(requestDto);

        var entity = await entityService.CreateAndSave(createDto);

        return CreatedAtAction(
            nameof(Get),
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
    public async Task<ActionResult> Get(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }

    [HttpPatch("{id:guid}", Name = "RecipePatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] RecipeUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateEntityDto = TransformToUpdateDto(requestDto);

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
    public async Task<ActionResult> UpdateCategories(Guid id, [FromBody] CategoriesUpdateRecipeRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateCategoriesDto = TransformToRecipeCategoriesCreateDto(requestDto);

        await entityService.UpdateCategories(entity, updateCategoriesDto);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }


    [HttpPost("{id:guid}/reviews", Name = "RecipeAddReview")]
    public async Task<ActionResult> AddReview(Guid id, [FromBody] ReviewCreateRequestDto createRequestDto)
    {
        var recipe = await entityService.Get(id);

        if (recipe == null)
        {
            return NotFound();
        }

        var createDto = mapper.Map<ReviewCreateDto>(createRequestDto);

        var review = await entityService.AddReview(recipe, createDto);

        return CreatedAtRoute(
            routeName: "ReviewGetByGuid",
            routeValues: new { id = review.Id },
            value: Transform(review)
        );
    }

    #endregion

    #region HELPERS

    private RecipeCreateDto TransformToCreateDto(RecipeCreateRequestDto requestDto) =>
        mapper.Map<RecipeCreateDto>(requestDto);

    private RecipeUpdateDto TransformToUpdateDto(RecipeUpdateRequestDto requestDto) =>
        mapper.Map<RecipeUpdateDto>(requestDto);


    private RecipeCategoriesDto TransformToRecipeCategoriesCreateDto(CategoriesUpdateRecipeRequestDto requestDto) =>
        mapper.Map<RecipeCategoriesDto>(requestDto);

    private ReviewResponseDto Transform(Review entity) =>
        Mapper.Map<ReviewResponseDto>(entity);

    #endregion
}