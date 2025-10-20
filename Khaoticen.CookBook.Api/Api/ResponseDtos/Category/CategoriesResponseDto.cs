using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Category;

public class CategoriesResponseDto: BaseEntitiesResponseDto
{
    public required string Name { get; set; } // todo: dodaj count recipes
}