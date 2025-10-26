namespace Khaoticen.CookBook.Api.Api.ResponseDtos.Shared.Base;

public abstract class BasePaginatedResponse<TEntityInListResponseDto>
    where TEntityInListResponseDto : BaseEntityInListResponseDto
{
    public List<TEntityInListResponseDto> Items { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}