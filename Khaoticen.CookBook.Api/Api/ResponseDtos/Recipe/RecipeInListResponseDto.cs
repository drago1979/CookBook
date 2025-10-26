using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Recipe;

public class RecipeInListResponseDto : BaseEntityInListResponseDto
{
    public required string Title { get; init; } // todo: dodaj count reviews
}