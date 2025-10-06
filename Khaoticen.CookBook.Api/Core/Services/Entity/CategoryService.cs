using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class CategoryService: BaseEntityService<
Category,
CategoryFactory,
CategoryCreateDto,
CategoryUpdateDto
>
{
    public CategoryService(AppDbContext db, CategoryFactory factory)
        : base(db, factory)
    {
    }
}