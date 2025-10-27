using System.ComponentModel.DataAnnotations;
using Khaoticen.CookBook.Api.Api.ValidationAttributes;
using Khaoticen.CookBook.Api.Shared.Query;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;

/// <summary>
///     A) SORTING:
///     Simply extend this class.
///
///     B) DEFAULT-SORTING:
///     Must add the sorting-column-name to specific EntityQueryMetadataClass (eg. CategoryQueryMetadata)
/// 
///     C) FILTERING
///
///         1 - In EntityQueryMetadataClass:
///             * Add the field.
///             * If you want validation too, do the "2"
/// 
///         2 - on child classes, add:
///
///         [AllowedValuesFromMetadata(typeof(CategoryQueryMetadata), nameof(CategoryQueryMetadata.Filters))]
///         [DependsOn(nameof(SearchValue))]
///         public string? SearchColumn { get; set; }
/// 
///         [DependsOn(nameof(SearchColumn))]
///         public string? SearchValue { get; set; }
///
/// </summary>
public abstract class BasePaginatedSortedRequest
{
    protected const int MaxPageSize = QueryConstants.MaxPageSize;
    private const int InitPageNumber = QueryConstants.InitPageNumber;
    private const string DefaultSortBy = QueryConstants.DefaultSortBy;


    [Range(1, int.MaxValue)]
    public virtual int Page { get; set; } = InitPageNumber;

    [Range(1, MaxPageSize)]
    public virtual int PageSize { get; set; } = MaxPageSize;

    [AllowedValuesFromMetadata(typeof(CategoryQueryMetadata), nameof(CategoryQueryMetadata.Sorts))]
    public virtual string SortBy { get; set; } = DefaultSortBy;
    public virtual SortDirection SortDirection { get; set; } = SortDirection.Asc;
}