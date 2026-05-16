using CreatorHub.Core.Enums;

namespace CreatorHub.Core.DTOs.Products;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int DownloadCount { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}