using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.Dtos.Response;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Reviews;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Update;
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

    [HttpGet("{id:guid}", Name = "ReviewGetByGuid")]
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntity(entity));
    }

    [HttpGet(Name = "ReviewGetAll")]
    public async Task<ActionResult> GetAll()
    {
        var entities = await entityService.GetAll();

        return Ok(TransformEntities(entities));
    }

    [HttpPatch("{id:guid}", Name = "ReviewPatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] ReviewUpdateDto entityUpdateDto,
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

    [HttpDelete("{id:guid}", Name = "ReviewDelete")]
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