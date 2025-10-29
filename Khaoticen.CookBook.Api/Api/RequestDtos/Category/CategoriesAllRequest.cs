using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Interface;
using Khaoticen.CookBook.Api.Api.ValidationAttributes;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Category;

public class CategoriesAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
{
    [AllowedValuesFromMetadata(typeof(CategoryQueryMetadata), nameof(CategoryQueryMetadata.Sorts))]
    public override string SortBy { get; set; } = DefaultSortBy;
    
    [AllowedValuesFromMetadata(typeof(CategoryQueryMetadata), nameof(CategoryQueryMetadata.Filters))]
    [DependsOn(nameof(SearchValue))]
    public string? SearchColumn { get; set; }

    [DependsOn(nameof(SearchColumn))]
    public string? SearchValue { get; set; }
}