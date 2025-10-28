using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Interface;
using Khaoticen.CookBook.Api.Api.ValidationAttributes;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Recipe;

public class RecipesAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
{
    [Range(1, MaxPageSize)]
    public override int PageSize { get; set; } = 10;

    [AllowedValuesFromMetadata(typeof(RecipeQueryMetadata), nameof(RecipeQueryMetadata.Sorts))]
    public override string SortBy { get; set; } = DefaultSortBy;

    [AllowedValuesFromMetadata(typeof(RecipeQueryMetadata), nameof(RecipeQueryMetadata.Filters))]
    [DependsOn(nameof(SearchValue))]
    public string? SearchColumn { get; set; }

    [DependsOn(nameof(SearchColumn))]
    public string? SearchValue { get; set; }

    // [Required]
    // [StringGuid]
    // public required string? CategoryId { get; set; } // todo!!!: check why was string
    public Guid? CategoryId { get; set; }
}
