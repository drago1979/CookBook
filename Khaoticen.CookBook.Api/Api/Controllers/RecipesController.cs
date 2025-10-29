using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;
using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController(IRecipeService entityService, IMapper mapper) : BaseAppController<
    Recipe,
    RecipeResponseDto,
    RecipeInListResponseDto,
    RecipesPaginatedResponseDto<RecipeInListResponseDto>,
    RecipesAllRequest
>(mapper)
{
    #region CRUD

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
    public async Task<ActionResult> GetAllAsync([FromQuery] RecipesAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request);

        return Ok(TransformToPaginated(request, items, total));
    }

    [HttpGet("with-deleted", Name = "RecipeGetAllWithDeleted")]
    public async Task<ActionResult> GetAllWithDeletedAsync([FromQuery] RecipesAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request, true);
        
        return Ok(TransformToPaginated(request, items, total));
    }

    [HttpGet("{id}", Name = "RecipeGet")]
    public async Task<ActionResult> GetAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);

        return Ok(TransformEntityToResponse(entity));
    }
    
    [HttpGet("{id}/with-deleted", Name = "RecipeGetWithDeleted")]
    public async Task<ActionResult> GetWithDeletedAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id, true);

        return Ok(TransformEntityToResponse(entity));
    }

    [HttpPatch("{id}", Name = "RecipePatch")]
    public async Task<ActionResult> PatchAsync(Guid id, [FromBody] RecipeUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await GetOrThrowAsync(id);

        var updateEntityDto = TransformToUpdateDto(requestDto);
        
        await entityService.UpdateAsync(updateEntityDto, entity);
        
        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }
    
    [HttpPatch("{id}/with-deleted", Name = "RecipePatchWithDeleted")]
    public async Task<ActionResult> PatchWithDeletedAsync(Guid id, [FromBody] RecipeUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await GetOrThrowAsync(id, true);

        var updateEntityDto = TransformToUpdateDto(requestDto);

        await entityService.UpdateAsync(updateEntityDto, entity);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }

    [HttpDelete("{id}", Name = "RecipeDelete")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);

        await entityService.DeleteAsync(entity);

        return NoContent();
    }

    #endregion

    #region RELATIONSHIPS

    [HttpPut("{id}/categories", Name = "RecipeUpdateCategories")]
    public async Task<ActionResult> UpdateCategoriesAsync(Guid id,
        [FromBody] RecipeCategoriesUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await GetOrThrowAsync(id);

        var updateCategoriesDto = TransformToRecipeCategoriesCreateDto(requestDto);

        await entityService.UpdateCategoriesAsync(entity, updateCategoriesDto);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }

    [HttpPost("{id}/reviews", Name = "RecipeAddReview")]
    public async Task<ActionResult> AddReviewAsync(Guid id, [FromBody] ReviewCreateRequestDto createRequestDto)
    {
        var entity = await GetOrThrowAsync(id);

        var createDto = Mapper.Map<ReviewCreateDto>(createRequestDto);

        var review = await entityService.AddReviewAsync(entity, createDto);

        return CreatedAtRoute(
            routeName: "ReviewGet",
            routeValues: new { id = review.Id },
            value: Transform(review)
        );
    }

    [HttpGet("{id}/reviews", Name = "RecipeGetReviews")]
    public async Task<ActionResult> GetReviewsAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);
        
        return Ok(TransformEntityToResponse(entity));
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

    private async Task<Recipe> GetOrThrowAsync(Guid id, bool withDeleted = false)
    {
        var entity = await entityService.GetWithRelatedAsync(id, withDeleted);

        if (entity is null) throw new EntityNotFoundException(nameof(Recipe), id.ToString());
        
        return entity;
    }

    #endregion
}