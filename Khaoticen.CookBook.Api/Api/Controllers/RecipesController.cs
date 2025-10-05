using System.Security.Cryptography.Xml;
using AutoMapper;
using Khaoticen.CookBook.Api.Api.Controllers.Shared.Base;
using Khaoticen.CookBook.Api.Api.Dtos.Response;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Create;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Update;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Services.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Khaoticen.CookBook.Api.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecipesController : AppControllerBase<
    Recipe,
    RecipeService,
    RecipeCreateDto,
    RecipeUpdateDto,
    RecipeResponseDto
>
{
    public RecipesController(RecipeService entityService, IMapper mapper) : base(entityService, mapper)
    {
    }

    [HttpPost("{id:guid}/reviews")]
    public async Task<ActionResult> AddReview(Guid id, [FromBody] ReviewCreateDto entityCreateDto)
    {
        var recipe = await EntityService.Get(id);
        
        if (recipe == null)
        {
            return NotFound();
        }
        
        var review = await EntityService.AddReview(recipe, entityCreateDto);
       
        return CreatedAtAction(
            nameof(GetByGuid), // this exists in the base controller
            new { id = review.Id }, 
            Transform(review)
        );
    }

    private ReviewResponseDto Transform(Review entity)
    {
        return Mapper.Map<ReviewResponseDto>(entity);
    }
}