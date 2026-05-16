using CreatorHub.Core.DTOs.Products;

namespace CreatorHub.Core.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto, Guid sellerId);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProductResponseDto>> GetBySellerAsync(Guid sellerId);
    Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto dto, Guid sellerId);
    Task DeleteAsync(Guid id, Guid sellerId);
}