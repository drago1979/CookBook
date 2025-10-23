using Khaoticen.CookBook.Api.Core.Dtos.Entity.Shared.Interface;

namespace Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;

public class CategoryUpdateDto: IUpdateDto
{
    public required string Name { get; set; }
}