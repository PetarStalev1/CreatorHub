namespace CreatorHub.Core.Entities;

public class Review : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid ReviewerId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public Product Product { get; set; } = null!;
    public User Reviewer { get; set; } = null!;
}