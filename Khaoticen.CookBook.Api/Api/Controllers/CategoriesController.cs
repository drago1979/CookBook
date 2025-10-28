using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(ICategoryService entityService, IMapper mapper) : BaseAppController<
    Category,
    CategoryResponseDto,
    CategoryInListResponseDto,
    CategoriesPaginatedResponseDto<CategoryInListResponseDto>,
    CategoriesAllRequest
>(mapper)
{
    #region CRUD

    [HttpPost(Name = "CategoryCreate")]
    public async Task<ActionResult> CreateAsync([FromBody] CategoryCreateRequestDto requestDto)
    {
        var createEntityDto = TransformToCreateDto(requestDto);

        var entity = await entityService.CreateAndSaveAsync(createEntityDto);

        return CreatedAtRoute(
            "CategoryGet",
            new { id = entity.Id },
            TransformEntityToResponse(entity)
        );
    }

    [HttpGet(Name = "CategoryGetAll")]
    public async Task<ActionResult> GetAllAsync([FromQuery] CategoriesAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request);

        return Ok(TransformToPaginated(request, items, total));
    }

    [HttpGet("{id:guid}", Name = "CategoryGet")]
    public async Task<ActionResult> GetAsync(Guid id)
    {
        var entity = await entityService.GetWithRelatedAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }

    [HttpPatch("{id:guid}", Name = "CategoryPatch")]
    public async Task<ActionResult> PatchAsync(Guid id, [FromBody] CategoryUpdateRequestDto requestDto,
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

    [HttpDelete("{id:guid}", Name = "CategoryDelete")]
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

    #region OTHER

    [HttpGet("check-name", Name = "CategoryCheckName")]
    public async Task<ActionResult> CheckNameAsync(string name)
    {
        var entity = await entityService.GetByNameAsync(name);

        return Ok(entity != null);
    }

    #endregion


    #region HELPERS

    private CategoryCreateDto TransformToCreateDto(CategoryCreateRequestDto requestDto) =>
        Mapper.Map<CategoryCreateDto>(requestDto);

    private CategoryUpdateDto TransformToUpdateDto(CategoryUpdateRequestDto requestDto) =>
        Mapper.Map<CategoryUpdateDto>(requestDto);

    #endregion
}