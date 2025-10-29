using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewsController(IReviewService entityService, IMapper mapper) : BaseAppController<
    Review,
    ReviewResponseDto,
    ReviewInListResponseDto,
    ReviewsPaginatedResponseDto<ReviewInListResponseDto>,
    ReviewsAllRequest
>(mapper)
{
    #region CRUD

    /// <summary>
    /// Retrieves a paginated list of reviews based on the specified request.
    /// </summary>
    /// <param name="request">The request object containing filtering, sorting, and pagination parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ActionResult"/> with the paginated list of reviews.</returns>
    [HttpGet(Name = "ReviewGetAll")]
    public async Task<ActionResult> GetAllAsync([FromQuery] ReviewsAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request);
        
        return Ok(TransformToPaginated(request, items, total));
    }

    /// <summary>
    /// Updates an existing review with new data identified by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to update.</param>
    /// <param name="requestDto">The data used to update the review.</param>
    /// <param name="returnUpdated">Indicates whether the updated review should be returned in the response.</param>
    /// <returns>
    /// Returns <see cref="OkObjectResult"/> containing the updated review if <paramref name="returnUpdated"/> is true.
    /// Otherwise returns <see cref="NoContentResult"/> after successful update.
    /// </returns>
    [HttpPatch("{id}", Name = "ReviewPatch")]
    public async Task<ActionResult> PatchAsync(Guid id, [FromBody] ReviewUpdateRequestDto requestDto,
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
    /// Deletes a review identified by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the review to be deleted.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.</returns>
    [HttpDelete("{id}", Name = "ReviewDelete")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);

        await entityService.DeleteAsync(entity);

        return NoContent();
    }

    #endregion

    #region HELPERS

    private ReviewUpdateDto TransformToUpdateDto(ReviewUpdateRequestDto requestDto) =>
        Mapper.Map<ReviewUpdateDto>(requestDto);
    
    private async Task<Review> GetOrThrowAsync(Guid id)
    {
        var entity = await entityService.GetAsync(id);

        if (entity is null) throw new EntityNotFoundException(nameof(Review), id.ToString());
        
        return entity;
    }

    #endregion
}