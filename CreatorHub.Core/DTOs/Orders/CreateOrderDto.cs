namespace CreatorHub.Core.DTOs.Orders;

public class CreateOrderDto
{
    public List<Guid> ProductIds { get; set; } = new();
}