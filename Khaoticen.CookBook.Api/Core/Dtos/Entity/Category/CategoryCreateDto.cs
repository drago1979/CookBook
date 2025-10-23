using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;

public class CategoryCreateDto: ICreateDto
{
    public required string Name { get; set; }
}