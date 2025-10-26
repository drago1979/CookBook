using Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Interfaces;

public interface IPaginatedResponse
{
    public List<BaseEntityInListResponseDto> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}