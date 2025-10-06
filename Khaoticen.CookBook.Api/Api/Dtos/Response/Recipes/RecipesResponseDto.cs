using Khaoticen.CookBook.Api.Api.Dtos.Response.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.Dtos.Response.Recipes;

public class RecipesResponseDto : BaseEntitiesResponseDto
{
    public required string Title { get; init; } // todo: dodaj count reviews
}