using AutoMapper;
using Khaoticen.CookBook.Api.Core.Entities.Entity;
using Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Base;

public abstract class AppControllerBase<TEntity, TEntityService, TCreateDto, TUpdateDto, TResponseDto>(TEntityService entityService, IMapper mapper)
    : ControllerBase
    where TEntity : BaseEntity
    where TEntityService : IEntityService<TEntity,TCreateDto,TUpdateDto>
    where TCreateDto : class
    where TResponseDto : class
{
    #region CRUD

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] TCreateDto entityCreateDto)
    {
        var entity = await entityService.Create(entityCreateDto);

        return CreatedAtAction(
            nameof(GetByGuid),
            new { id = entity.Id },
            Transform(entity)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(Transform(entity));
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] TUpdateDto entityUpdateDto,
        bool returnUpdated = false)
    {
        // todo: u skriptu - debugging
        // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(entityCreateDto));
        var entity = await entityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        await entityService.Update(entity, entityUpdateDto);

        if (returnUpdated)
        {
            return Ok(Transform(entity));
        }

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
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
    
    #region ServiceMethods

    protected List<TResponseDto> Transform(IEnumerable<TEntity> entities)
    {
        return entities.Select(Transform).ToList();
    }

    protected TResponseDto Transform(TEntity entity)
    {
        return mapper.Map<TResponseDto>(entity);
    }

    #endregion
}