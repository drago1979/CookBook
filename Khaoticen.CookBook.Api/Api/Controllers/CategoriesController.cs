using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.Dtos.Response.Categories;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Recipe;
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
    public async Task<ActionResult> Create([FromBody] CategoryCreateDto entityCreateDto)
    {
        var entity = await entityService.CreateAndSave(entityCreateDto);

        return CreatedAtAction(
            nameof(GetByGuid),
            new { id = entity.Id },
            TransformEntity(entity)
        );
    }
    
    [HttpGet("{id:guid}", Name = "CategoryGetByGuid")]
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(TransformEntity(entity));
    }

    [HttpGet(Name = "CategoryGetAll")]
    public async Task<ActionResult> GetAll()
    {
        var entities = await entityService.GetAll();

        return Ok(TransformEntities(entities));
    }
    
    [HttpPatch("{id:guid}", Name = "CategoryPatch")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] CategoryUpdateDto entityUpdateDto,
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
    
    #region RELATIONSHIPS
    #endregion

    #region OTHER

    // [HttpGet("/check-name", Name = "CategoryCheckName")]
    // public async Task<ActionResult> CheckName(string name)
    // {
    //     var entity = await entityService.Get(name);
    //     
    //     return Ok(entity != null);
    // }

    #endregion
    
}