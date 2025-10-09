using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.Dtos.Request.Category;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Categories;
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
    public async Task<ActionResult> Create([FromBody] CategoryCreateRequest request)
    {
        var createEntityDto = TransformToCreateDto(request);
        
        var entity = await entityService.CreateAndSave(createEntityDto);

        return CreatedAtAction(
            nameof(GetByGuid),
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
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntityToResponse(entity));
    }
    
    [HttpPatch("{id:guid}", Name = "CategoryPatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] CategoryUpdateRequest request,
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

    private CategoryCreateDto TransformToCreateDto(CategoryCreateRequest request)
    {
        return mapper.Map<CategoryCreateDto>(request);
    }
    
    private CategoryUpdateDto TransformToUpdateDto(CategoryUpdateRequest request)
    {
        return mapper.Map<CategoryUpdateDto>(request);
    }

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

