using CreatorHub.Core.DTOs.Orders;
using CreatorHub.Core.Entities;
using CreatorHub.Core.Interfaces;
using CreatorHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreatorHub.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto, Guid buyerId)
    {
        var products = await _context.Products
            .Where(p => dto.ProductIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != dto.ProductIds.Count)
            throw new Exception("Някои продукти не са намерени.");

        var order = new Order
        {
            BuyerId = buyerId,
            TotalAmount = products.Sum(p => p.Price),
            Status = Core.Enums.OrderStatus.Completed
        };

        _context.Orders.Add(order);

        var orderItems = products.Select(p => new OrderItem
        {
            OrderId = order.Id,
            ProductId = p.Id,
            PriceAtPurchase = p.Price
        }).ToList();

        _context.OrderItems.AddRange(orderItems);

        foreach (var product in products)
            product.DownloadCount++;

        await _context.SaveChangesAsync();

        return MapToDto(order, orderItems, products);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetByBuyerAsync(Guid buyerId)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Where(o => o.BuyerId == buyerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(o => MapToDto(
            o,
            o.OrderItems.ToList(),
            o.OrderItems.Select(oi => oi.Product).ToList()
        ));
    }

    public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        return MapToDto(
            order,
            order.OrderItems.ToList(),
            order.OrderItems.Select(oi => oi.Product).ToList()
        );
    }

    private OrderResponseDto MapToDto(Order order, List<OrderItem> items, List<Product> products)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            Items = items.Select(oi =>
            {
                var product = products.FirstOrDefault(p => p.Id == oi.ProductId);
                return new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductTitle = product?.Title ?? string.Empty,
                    PriceAtPurchase = oi.PriceAtPurchase
                };
            }).ToList()
        };
    }
}