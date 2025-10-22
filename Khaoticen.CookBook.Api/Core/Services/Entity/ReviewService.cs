using AutoMapper;
using Khaoticen.CookBook.Api.Core.Dtos.Entity.Review;
using Khaoticen.CookBook.Api.Core.Entities;
using Khaoticen.CookBook.Api.Core.Factories;
using Khaoticen.CookBook.Api.Core.Repositories;
using Khaoticen.CookBook.Api.Core.Services.Entity.Shared.Base;
using Khaoticen.CookBook.Api.Infrastructure.Db;

namespace Khaoticen.CookBook.Api.Core.Services.Entity;

public class ReviewService : BaseEntityService<
    Review,
    Factories.ReviewFactory,
    ReviewCreateDto,
    ReviewUpdateDto
>
{
    private readonly ReviewRepository _repository;
    private readonly IMapper _mapper;

    public ReviewService(AppDbContext db, Factories.ReviewFactory factory, ReviewRepository repository, IMapper mapper)
        : base(db, factory)
    {
        _repository = repository;
        _mapper = mapper;
    }

    #region CRUD

    public async Task<Review?> GetAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task UpdateAsync(ReviewUpdateDto dto, Review entity)
    {
        _mapper.Map(dto, entity);
        
        _repository.Update(entity);
        
        await Db.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Review entity)
    {
        _repository.Delete(entity);

        await Db.SaveChangesAsync();
    }
    #endregion
}