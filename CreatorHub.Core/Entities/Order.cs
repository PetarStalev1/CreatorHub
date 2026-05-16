using CreatorHub.Core.Enums;

namespace CreatorHub.Core.Entities;

public class Order : BaseEntity
{
    public Guid BuyerId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? StripePaymentId { get; set; }

    public User Buyer { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}