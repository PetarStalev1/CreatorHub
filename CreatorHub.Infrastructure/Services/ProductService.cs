using CreatorHub.Core.DTOs.Products;
using CreatorHub.Core.Entities;
using CreatorHub.Core.Interfaces;
using CreatorHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreatorHub.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto, Guid sellerId)
    {
        var product = new Product
        {
            SellerId = sellerId,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            FileUrl = dto.FileUrl,
            Category = dto.Category
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return await MapToDto(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _context.Products
            .Include(p => p.Seller)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return products.Select(p => MapToDto(p).Result);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) return null;

        return await MapToDto(product);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetBySellerAsync(Guid sellerId)
    {
        var products = await _context.Products
            .Include(p => p.Seller)
            .Where(p => p.SellerId == sellerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return products.Select(p => MapToDto(p).Result);
    }

    public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto dto, Guid sellerId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new Exception("Продуктът не е намерен.");

        if (product.SellerId != sellerId)
            throw new Exception("Нямаш права да редактираш този продукт.");

        if (dto.Title != null) product.Title = dto.Title;
        if (dto.Description != null) product.Description = dto.Description;
        if (dto.Price != null) product.Price = dto.Price.Value;

        await _context.SaveChangesAsync();

        return await MapToDto(product);
    }

    public async Task DeleteAsync(Guid id, Guid sellerId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new Exception("Продуктът не е намерен.");

        if (product.SellerId != sellerId)
            throw new Exception("Нямаш права да изтриеш този продукт.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    private async Task<ProductResponseDto> MapToDto(Product product)
    {
        if (product.Seller == null)
        {
            await _context.Entry(product).Reference(p => p.Seller).LoadAsync();
        }

        return new ProductResponseDto
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            FileUrl = product.FileUrl,
            Category = product.Category.ToString(),
            DownloadCount = product.DownloadCount,
            SellerName = product.Seller?.DisplayName ?? string.Empty,
            CreatedAt = product.CreatedAt
        };
    }
}