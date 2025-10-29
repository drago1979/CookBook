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

/// Controller for handling operations related to recipes.
/// Inherits from BaseAppController.
/// Provides functionalities such as creating, retrieving, updating, and deleting recipes, as well as handling associated data like reviews and categories.
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

    /// <summary>
    /// Creates a new recipe based on the provided data.
    /// </summary>
    /// <param name="requestDto">The data transfer object containing the details of the recipe to create.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the creation process. If successful, a response with a link to the created recipe is returned.</returns>
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

    /// <summary>
    /// Retrieves a paginated and sorted list of all recipes based on the given request parameters.
    /// </summary>
    /// <param name="request">The paginated and sorted request object containing filtering criteria and other parameters for retrieving the recipes.</param>
    /// <returns>A HTTP response containing a paginated list of recipes with their total count.</returns>
    [HttpGet(Name = "RecipeGetAll")]
    public async Task<ActionResult> GetAllAsync([FromQuery] RecipesAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request);

        return Ok(TransformToPaginated(request, items, total));
    }

    /// <summary>
    /// Retrieves all recipes including the ones marked as deleted based on the provided request parameters.
    /// </summary>
    /// <param name="request">The request object containing filtering, sorting, and pagination parameters.</param>
    /// <returns>An action result containing the paginated list of recipes, including deleted entries, and the total count.</returns>
    [HttpGet("with-deleted", Name = "RecipeGetAllWithDeleted")]
    public async Task<ActionResult> GetAllWithDeletedAsync([FromQuery] RecipesAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request, true);
        
        return Ok(TransformToPaginated(request, items, total));
    }

    /// <summary>
    /// Retrieves the details of a specific recipe by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <returns>An <see cref="ActionResult"/> containing the recipe details if found. If the recipe does not exist, a not-found response is returned.</returns>
    [HttpGet("{id}", Name = "RecipeGet")]
    public async Task<ActionResult> GetAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);

        return Ok(TransformEntityToResponse(entity));
    }

    /// <summary>
    /// Retrieves a recipe by the specified ID, including recipes that have been marked as deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to retrieve.</param>
    /// <returns>An <see cref="ActionResult"/> containing the requested recipe data if found, or a response indicating the recipe does not exist.</returns>
    [HttpGet("{id}/with-deleted", Name = "RecipeGetWithDeleted")]
    public async Task<ActionResult> GetWithDeletedAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id, true);

        return Ok(TransformEntityToResponse(entity));
    }

    /// <summary>
    /// Updates the details of an existing recipe identified by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to be updated.</param>
    /// <param name="requestDto">The data transfer object containing the updated details for the recipe.</param>
    /// <param name="returnUpdated">Indicates whether to return the updated entity in the response.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the update operation. Returns the updated recipe if <paramref name="returnUpdated"/> is true; otherwise, no content is returned.</returns>
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

    /// <summary>
    /// Updates an existing recipe, including those marked as deleted, based on the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to update.</param>
    /// <param name="requestDto">The data transfer object containing the updated recipe details.</param>
    /// <param name="returnUpdated">A boolean indicating whether to return the updated recipe in the response.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the update operation. If <paramref name="returnUpdated"/> is true, the updated recipe is returned; otherwise, a "No Content" response is sent.</returns>
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

    /// <summary>
    /// Deletes a specific recipe identified by the given ID.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to delete.</param>
    /// <returns>A <see cref="ActionResult"/> with no content if the deletion is successful.</returns>
    [HttpDelete("{id}", Name = "RecipeDelete")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);

        await entityService.DeleteAsync(entity);

        return NoContent();
    }

    #endregion

    #region RELATIONSHIPS

    /// <summary>
    /// Updates the categories associated with a specific recipe.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to update.</param>
    /// <param name="requestDto">The data transfer object containing the updated categories for the recipe.</param>
    /// <param name="returnUpdated">A flag indicating whether to return the updated recipe in the response.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the update operation. If <paramref name="returnUpdated"/> is true, the updated recipe details are returned; otherwise, a no-content response is returned.</returns>
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

    /// <summary>
    /// Adds a new review to an existing recipe based on the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe to which the review will be added.</param>
    /// <param name="createRequestDto">The data transfer object containing the details of the review to create.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the addition process. If successful, a response with a link to the newly created review is returned.</returns>
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

    /// <summary>
    /// Retrieves the reviews associated with a specific recipe.
    /// </summary>
    /// <param name="id">The unique identifier of the recipe for which the reviews are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains an ActionResult wrapping the response with the retrieved reviews.</returns>
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