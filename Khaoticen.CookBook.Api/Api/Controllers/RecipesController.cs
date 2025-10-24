using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController(IRecipeService entityService, IMapper mapper) : AppControllerBase<
    Recipe,
    RecipeResponseDto,
    RecipesResponseDto
>(mapper)
{
    #region CRUD

    /// <summary>
    /// Accepts multiple Categories
    /// </summary>
    /// <param name="requestDto"></param>
    /// <returns></returns>
    [HttpPost(Name = "RecipeCreate")]
    public async Task<ActionResult> CreateAsync([FromBody] RecipeCreateRequestDto requestDto)
    {
        var createDto = TransformToCreateDto(requestDto);

        var entity = await entityService.CreateAndSaveAsync(createDto);

        return CreatedAtRoute(
            "RecipeGet",
            new { id = entity.Id },
            TransformEntityToResponse(entity)
        );
    }

    [HttpGet(Name = "RecipeGetAll")]
    public async Task<ActionResult> GetAllAsync()
    {
        var entities = await entityService.GetAllAsync();

        return Ok(TransformEntitiesToResponse(entities));
    }

    [HttpGet("all", Name = "RecipeGetAllWithDeleted")]
    public async Task<ActionResult> GetAllWithDeletedAsync()
    {
        var entities = await entityService.GetAllWithDeletedAsync();

        return Ok(TransformEntitiesToResponse(entities));
    }
    
    [HttpGet("{id:guid}", Name = "RecipeGet")]
    public async Task<ActionResult> GetAsync(Guid id)
    {
        var entity = await entityService.GetWithRelatedAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }

    [HttpPatch("{id:guid}", Name = "RecipePatch")]
    public async Task<ActionResult> PatchAsync(Guid id, [FromBody] RecipeUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await entityService.GetWithRelatedAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateEntityDto = TransformToUpdateDto(requestDto);
    
        await entityService.UpdateAsync(updateEntityDto, entity);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "RecipeDelete")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var entity = await entityService.GetAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        await entityService.DeleteAsync(entity);

        return NoContent();
    }

    #endregion

    #region RELATIONSHIPS

    [HttpPut("{id:guid}/categories", Name = "RecipeUpdateCategories")]
    public async Task<ActionResult> UpdateCategoriesAsync(Guid id, [FromBody] RecipeCategoriesUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await entityService.GetAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateCategoriesDto = TransformToRecipeCategoriesCreateDto(requestDto);

        await entityService.UpdateCategoriesAsync(entity, updateCategoriesDto);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }


    [HttpPost("{id:guid}/reviews", Name = "RecipeAddReview")]
    public async Task<ActionResult> AddReviewAsync(Guid id, [FromBody] ReviewCreateRequestDto createRequestDto)
    {
        var recipe = await entityService.GetAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }

        var createDto = Mapper.Map<ReviewCreateDto>(createRequestDto);

        var review = await entityService.AddReviewAsync(recipe, createDto);

        return CreatedAtRoute(
            routeName: "ReviewGet",
            routeValues: new { id = review.Id },
            value: Transform(review)
        );
    }

    #endregion

    #region HELPERS

    private RecipeCreateDto TransformToCreateDto(RecipeCreateRequestDto requestDto) =>
        Mapper.Map<RecipeCreateDto>(requestDto);

    private RecipeUpdateDto TransformToUpdateDto(RecipeUpdateRequestDto requestDto) =>
        Mapper.Map<RecipeUpdateDto>(requestDto);


    private RecipeCategoriesDto TransformToRecipeCategoriesCreateDto(RecipeCategoriesUpdateRequestDto requestDto) =>
        Mapper.Map<RecipeCategoriesDto>(requestDto);

    private ReviewResponseDto Transform(Review entity) =>
        Mapper.Map<ReviewResponseDto>(entity);

    #endregion
}