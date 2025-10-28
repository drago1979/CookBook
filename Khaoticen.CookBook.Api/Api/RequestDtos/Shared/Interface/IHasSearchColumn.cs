namespace Khaoticen.CookBook.Api.Api.RequestDtos.Shared.Interface;

public interface IHasSearchColumn
{
    string? SearchColumn { get; }
    string? SearchValue  { get; }
}