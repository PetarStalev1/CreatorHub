using CreatorHub.Core.Enums;

namespace CreatorHub.Core.DTOs.Products;

public class CreateProductDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public ProductCategory Category { get; set; }
}