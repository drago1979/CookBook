using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(CategoryService entityService, IMapper mapper) : AppControllerBase<
    Category,
    CategoryResponseDto,
    CategoriesResponseDto
>(mapper)
{
    #region CRUD

    [HttpPost(Name = "CategoryCreate")]
    public async Task<ActionResult> Create([FromBody] CategoryCreateRequestDto requestDto)
    {
        var createEntityDto = TransformToCreateDto(requestDto);

        var entity = await entityService.CreateAndSave(createEntityDto);

        return CreatedAtAction(
            nameof(Get),
            new { id = entity.Id },
            TransformEntityToResponse(entity)
        );
    }

    [HttpGet(Name = "CategoryGetAll")]
    public async Task<ActionResult> GetAll()
    {
        var entities = await entityService.GetAll();

        return Ok(TransformEntitiesToResponse(entities));
    }


    [HttpGet("{id:guid}", Name = "CategoryGetByGuid")]
    public async Task<ActionResult> Get(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }

    [HttpPatch("{id:guid}", Name = "CategoryPatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] CategoryUpdateRequestDto requestDto,
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

    [HttpDelete("{id:guid}", Name = "CategoryDelete")]
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

    private CategoryCreateDto TransformToCreateDto(CategoryCreateRequestDto requestDto) =>
        mapper.Map<CategoryCreateDto>(requestDto);

    private CategoryUpdateDto TransformToUpdateDto(CategoryUpdateRequestDto requestDto) =>
        mapper.Map<CategoryUpdateDto>(requestDto);

    #endregion

    #region OTHER

    [HttpGet("/check-name", Name = "CategoryCheckName")]
    public async Task<ActionResult> CheckName(string name)
    {
        var entity = await entityService.GetByName(name);

        return Ok(entity != null);
    }

    #endregion
}