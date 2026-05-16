using CreatorHub.Core.Enums;

namespace CreatorHub.Core.Entities;

public class Product : BaseEntity
{
    public Guid SellerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public ProductCategory Category { get; set; }
    public int DownloadCount { get; set; } = 0;

    public User Seller { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}