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
public class ReviewsController : AppControllerBase <
    Review,
    ReviewService,
    ReviewCreateDto,
    ReviewUpdateDto,
    ReviewResponseDto
>
{
    public ReviewsController(ReviewService entityService, IMapper mapper) : base(entityService, mapper)
    {
    }
    
    [NonAction] // todo: IgnoreApi = true
    public override Task<ActionResult> Create([FromBody] ReviewCreateDto dto)
    {
        return base.Create(dto);
    }
    
}