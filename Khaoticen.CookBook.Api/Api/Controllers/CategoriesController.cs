using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Category;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Category;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Exceptions;
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

    /// <summary>
    /// Creates a new category from the provided request data and saves it to the database.
    /// Returns a response indicating the resource creation status.
    /// </summary>
    /// <param name="requestDto">The data required to create a new category.</param>
    /// <returns>An action result containing the details of the created category,
    /// or an appropriate status code if an error occurs.</returns>
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

    /// <summary>
    /// Retrieves a paginated and optionally filtered list of categories based on the provided request parameters.
    /// </summary>
    /// <param name="request">The request object containing pagination, sorting, and filtering criteria for retrieving the categories.</param>
    /// <returns>An action result containing the paginated list of categories and the total count of matched items.</returns>
    [HttpGet(Name = "CategoryGetAll")]
    public async Task<ActionResult> GetAllAsync([FromQuery] CategoriesAllRequest request)
    {
        var (items, total) = await entityService.GetWithCountAsync(request);
    
        return Ok(TransformToPaginated(request, items, total));
    }

    /// <summary>
    /// Retrieves a single category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the category to retrieve.</param>
    /// <returns>An <see cref="ActionResult"/> containing the details of the category if found, or an appropriate status code if not.</returns>
    [HttpGet("{id}", Name = "CategoryGet")]
    public async Task<ActionResult> GetAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);
        
        return Ok(TransformEntityToResponse(entity));
    }

    /// <summary>
    /// Updates an existing category with the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the category to be updated.</param>
    /// <param name="requestDto">The data transfer object containing the updated values for the category.</param>
    /// <param name="returnUpdated">Indicates whether the response should include the updated category data. Defaults to false.</param>
    /// <returns>An <see cref="ActionResult"/> indicating the result of the operation.
    /// If <paramref name="returnUpdated"/> is true, returns the updated category data. Otherwise, returns a no-content result.</returns>
    [HttpPatch("{id}", Name = "CategoryPatch")]
    public async Task<ActionResult> PatchAsync(Guid id, [FromBody] CategoryUpdateRequestDto requestDto,
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
    /// Deletes a category identified by the specified ID from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the category to be deleted.</param>
    /// <returns>A no-content response indicating successful deletion, or an appropriate error response if the category is not found or cannot be deleted.</returns>
    [HttpDelete("{id}", Name = "CategoryDelete")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        var entity = await GetOrThrowAsync(id);
        
        await entityService.DeleteAsync(entity);

        return NoContent();
    }

    #endregion

    #region OTHER

    /// <summary>
    /// Checks if a category with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the category to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether a category with the specified name exists (true) or not (false).</returns>
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
    
    private async Task<Category> GetOrThrowAsync(Guid id)
    {
        var entity = await entityService.GetWithRelatedAsync(id);

        if (entity is null) throw new EntityNotFoundException(nameof(Category), id.ToString());
        
        return entity;
    }

    #endregion
}