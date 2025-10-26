using Khaoticen.CookBook.Api.Api.RequestDtos.Review;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;
using Khaoticen.CookBook.Api.Api.ResponseDtos.Review;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Shared.Mappings.Entities.Shared;

namespace Khaoticen.CookBook.Api.Shared.Mappings.Entities;

public class ReviewProfile : BaseEntityProfile<
    Review,
    ReviewCreateRequestDto,
    ReviewUpdateRequestDto,
    ReviewCreateDto,
    ReviewUpdateDto,
    ReviewResponseDto,
    ReviewInListResponseDto
>
{
    public ReviewProfile()
    {
        // ENTITIES => RESPONSES
        // RELATIONSHIPS - include
        CreateMap<Review, ReviewResponseDto>()
            .ForMember(dest => dest.Recipe, opt => opt.MapFrom(src => src.Recipe));
        
        // Entity in relationship
        CreateMap<Review, ReviewInRelatedResponseDto>();
    }
}