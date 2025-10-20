using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;

public class RecipesResponseDto : BaseEntitiesResponseDto
{
    public required string Title { get; init; } // todo: dodaj count reviews
}