namespace CreatorHub.Core.DTOs.Reviews;

public class CreateReviewDto
{
    public Guid ProductId { get; set; }
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
}