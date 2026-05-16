namespace CreatorHub.Core.DTOs.Reviews;

public class ReviewResponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}