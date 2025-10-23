using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Category;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories.Shared.Base;

namespace Khaoticen.CookBook.Api.Core.Factories;

public class CategoryFactory(IMapper mapper) : BaseFactory<Category, CategoryCreateDto>(mapper);