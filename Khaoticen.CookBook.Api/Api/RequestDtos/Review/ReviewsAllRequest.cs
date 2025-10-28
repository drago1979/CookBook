using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Base;
using Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Interface;
using Khaoticen.CookBook.Api.Api.ValidationAttributes;
using Khaoticen.CookBook.Api.Shared.Query.Metadata;

namespace Khaoticen.CookBook.Api.Api.RequestDtos.Review;

public class ReviewsAllRequest : BasePaginatedSortedRequest, IHasSearchColumn
{
    [AllowedValuesFromMetadata(typeof(ReviewQueryMetadata), nameof(ReviewQueryMetadata.Filters))]
    [DependsOn(nameof(SearchValue))]
    public string? SearchColumn { get; set; }

    [DependsOn(nameof(SearchColumn))]
    public string? SearchValue { get; set; }
}