using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Api.ValidationAttributes;
using Khaoticen.CookBook.Api.Shared.Query;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;

public abstract class BasePaginatedSortedRequest
{
    protected const int MaxPageSize = QueryConstants.MaxPageSize;
    protected const int InitPageNumber = QueryConstants.InitPageNumber;
    protected const string DefaultSortBy = QueryConstants.DefaultSortBy;


    [Range(1, int.MaxValue)]
    public int Page { get; set; } = InitPageNumber;

    [Range(1, MaxPageSize)]
    public int PageSize { get; set; } = MaxPageSize;
    

    [AllowedValuesFromMetadata(typeof(CategoryQueryMetadata), nameof(CategoryQueryMetadata.Sorts))]
    public string SortBy { get; set; } = DefaultSortBy;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}