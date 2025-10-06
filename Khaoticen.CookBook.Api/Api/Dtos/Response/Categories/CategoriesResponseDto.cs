using Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Categories;

public class CategoriesResponseDto: BaseEntitiesResponseDto
{
    public required string Name { get; set; } // todo: dodaj count recipes
}