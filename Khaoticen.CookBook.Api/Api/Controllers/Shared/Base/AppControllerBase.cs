using AutoMapper;
using Khaoticen.CookBook.Api.Core.Entities.Shared.Base;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;

public abstract class AppControllerBase<
    TEntity,
    TEntityService,
    TCreateDto,
    TUpdateDto,
    TResponseDto
>
    : ControllerBase
    where TEntity : BaseEntity
    where TEntityService : IEntityService<TEntity, TCreateDto, TUpdateDto>
    where TCreateDto : class
    where TResponseDto : class
{
    protected readonly TEntityService EntityService;
    protected readonly IMapper Mapper;

    public AppControllerBase(TEntityService entityService, IMapper mapper)
    {
        EntityService = entityService;
        Mapper = mapper;
    }

    #region CRUD

    [HttpPost]
    public virtual async Task<ActionResult> Create([FromBody] TCreateDto entityCreateDto)
    {
        var entity = await EntityService.CreateAsync(entityCreateDto);

        return CreatedAtAction(
            nameof(GetByGuid),
            new { id = entity.Id },
            Transform(entity)
        );
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetByGuid(Guid id)
    {
        var entity = await EntityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        return Ok(Transform(entity));
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var entities = await EntityService.GetAll();

        return Ok(Transform(entities));
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult> Patch(Guid id, [FromBody] TUpdateDto entityUpdateDto,
        bool returnUpdated = false)
    {
        // todo: u skriptu - debugging
        // Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(entityCreateDto));
        var entity = await EntityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        await EntityService.Update(entity, entityUpdateDto);

        if (returnUpdated)
        {
            return Ok(Transform(entity));
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id) // todo: dodaj createdAt, updatedAt u response
    {
        var entity = await EntityService.Get(id);

        if (entity == null)
        {
            return NotFound();
        }

        await EntityService.Delete(entity);

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
        return Mapper.Map<TResponseDto>(entity);
    }

    #endregion
}