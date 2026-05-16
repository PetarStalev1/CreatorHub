using CreatorHub.Core.DTOs.Orders;
using CreatorHub.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CreatorHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // POST api/orders - създай поръчка
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        try
        {
            var buyerId = GetCurrentUserId();
            if (buyerId == null)
                return Unauthorized();

            var order = await _orderService.CreateAsync(dto, buyerId.Value);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET api/orders - поръчките на логнатия user
    [HttpGet]
    public async Task<IActionResult> GetMy()
    {
        var buyerId = GetCurrentUserId();
        if (buyerId == null)
            return Unauthorized();

        var orders = await _orderService.GetByBuyerAsync(buyerId.Value);
        return Ok(orders);
    }

    // GET api/orders/{id} - конкретна поръчка
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound(new { message = "Поръчката не е намерена." });

        return Ok(order);
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(claim, out var id))
            return id;
        return null;
    }
}