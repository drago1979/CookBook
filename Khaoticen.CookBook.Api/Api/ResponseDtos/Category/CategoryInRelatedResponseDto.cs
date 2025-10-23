using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Category;

public class CategoryInRelatedResponseDto : BaseEntityInRelatedResponseDto
{
    public required string Name { get; set; }
}