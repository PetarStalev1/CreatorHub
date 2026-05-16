using CreatorHub.Core.DTOs.Reviews;
using CreatorHub.Core.Entities;
using CreatorHub.Core.Interfaces;
using CreatorHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreatorHub.Infrastructure.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _context;

    public ReviewService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReviewResponseDto> CreateAsync(CreateReviewDto dto, Guid reviewerId)
    {
        if (dto.Rating < 1 || dto.Rating > 5)
            throw new Exception("Оценката трябва да е между 1 и 5.");

        var alreadyReviewed = await _context.Reviews
            .AnyAsync(r => r.ProductId == dto.ProductId && r.ReviewerId == reviewerId);

        if (alreadyReviewed)
            throw new Exception("Вече си оставил оценка за този продукт.");

        var review = new Review
        {
            ProductId = dto.ProductId,
            ReviewerId = reviewerId,
            Rating = dto.Rating,
            Comment = dto.Comment
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var reviewer = await _context.Users.FindAsync(reviewerId);

        return new ReviewResponseDto
        {
            Id = review.Id,
            ProductId = review.ProductId,
            ReviewerName = reviewer?.DisplayName ?? string.Empty,
            Rating = review.Rating,
            Comment = review.Comment,
            CreatedAt = review.CreatedAt
        };
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetByProductAsync(Guid productId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.Reviewer)
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reviews.Select(r => new ReviewResponseDto
        {
            Id = r.Id,
            ProductId = r.ProductId,
            ReviewerName = r.Reviewer?.DisplayName ?? string.Empty,
            Rating = r.Rating,
            Comment = r.Comment,
            CreatedAt = r.CreatedAt
        });
    }

    public async Task DeleteAsync(Guid id, Guid reviewerId)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
            throw new Exception("Ревюто не е намерено.");

        if (review.ReviewerId != reviewerId)
            throw new Exception("Нямаш права да изтриеш това ревю.");

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}