namespace CreatorHub.Core.DTOs.Orders;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;
    public decimal PriceAtPurchase { get; set; }
}