using CreatorHub.Core.DTOs.Orders;

namespace CreatorHub.Core.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(CreateOrderDto dto, Guid buyerId);
    Task<IEnumerable<OrderResponseDto>> GetByBuyerAsync(Guid buyerId);
    Task<OrderResponseDto?> GetByIdAsync(Guid id);
}