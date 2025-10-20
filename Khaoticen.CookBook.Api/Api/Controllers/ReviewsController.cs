using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewsController(ReviewService entityService, IMapper mapper) : AppControllerBase<
    Review,
    ReviewResponseDto,
    ReviewsResponseDto
>(mapper)
{
    #region CRUD

    [HttpGet(Name = "ReviewGetAll")]
    public async Task<ActionResult> GetAll()
    {
        var entities = await entityService.GetAll();

        return Ok(TransformEntitiesToResponse(entities));
    }
    
    [HttpGet("{id:guid}", Name = "ReviewGetByGuid")]
    public async Task<ActionResult> Get(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }
    
    [HttpPatch("{id:guid}", Name = "ReviewPatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] ReviewUpdateRequestDto requestDto,
        bool returnUpdated = false)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        var updateEntityDto =  TransformToUpdateDto(requestDto);
        
        await entityService.Update(entity, updateEntityDto);

        if (returnUpdated)
        {
            return Ok(TransformEntityToResponse(entity));
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "ReviewDelete")]
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

    #region HELPERS

    private ReviewUpdateDto TransformToUpdateDto(ReviewUpdateRequestDto requestDto)
    {
        return mapper.Map<ReviewUpdateDto>(requestDto);
    }

    #endregion
}