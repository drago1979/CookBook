// using AutoMapper;
// using Khaoticen.CookBook.Api.Api.Controllers.Base;
// using Khaoticen.CookBook.Api.Api.DTOs;
// using Khaoticen.CookBook.Api.Core.Dtos.Reviews;
// using Khaoticen.CookBook.Api.Core.Entities;
// using Khaoticen.CookBook.Api.Core.Services.Entity.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Khaoticen.CookBook.Api.Api.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class ReviewsController(IMapper mapper, IReviewService entityService)
//     : AppControllerBase<Review, IReviewService, ReviewCreateDto, ReviewResponseDto>(mapper)
// {
//     #region CRUD
//
//     [HttpPost]
//     public async Task<ActionResult> Create([FromBody] ReviewCreateDto entityCreateDto)
//     {
//         var entity = await entityService.Create(entityCreateDto);
//
//         return CreatedAtAction(
//             nameof(GetByGuid),
//             new { id = entity.Id },
//             Transform(entity)
//         );
//     }
//
//     [HttpGet("{id:guid}")]
//     public async Task<ActionResult> GetByGuid(Guid id)
//     {
//         var entity = await entityService.Get(id);
//
//         if (entity == null)
//         {
//             return NotFound();
//         }
//
//         return Ok(Transform(entity));
//     }
//
//     #endregion
// }