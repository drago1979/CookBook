using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Reviews;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories.Base;

namespace Khaoticen.CookBook.Api.Core.Factories;

public class ReviewFactory : BaseFactory<Recipe, ReviewCreateDto, ReviewUpdateDto>
{
    public ReviewFactory(IMapper mapper): base(mapper)
    {
    }
    
    // public void Update(ReviewUpdateDto dto, Review entity)
    // {
    //     mapper.Map(dto, entity);
    //
    //     // Domain rules / overrides
    //     // entity.Title = dto.Title.Trim();          // normalization
    //     // entity.CreatedAt = DateTime.UtcNow;       // system-set field
    // }
}