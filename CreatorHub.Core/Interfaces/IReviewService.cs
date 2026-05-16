using CreatorHub.Core.DTOs.Reviews;

namespace CreatorHub.Core.Interfaces;

public interface IReviewService
{
    Task<ReviewResponseDto> CreateAsync(CreateReviewDto dto, Guid reviewerId);
    Task<IEnumerable<ReviewResponseDto>> GetByProductAsync(Guid productId);
    Task DeleteAsync(Guid id, Guid reviewerId);
}