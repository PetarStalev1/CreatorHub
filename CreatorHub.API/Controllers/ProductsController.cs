using CreatorHub.Core.DTOs.Products;
using CreatorHub.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CreatorHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Продуктът не е намерен." });

        return Ok(product);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMy()
    {
        var sellerId = GetCurrentUserId();
        if (sellerId == null)
            return Unauthorized();

        var products = await _productService.GetBySellerAsync(sellerId.Value);
        return Ok(products);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var sellerId = GetCurrentUserId();
            if (sellerId == null)
                return Unauthorized();

            var product = await _productService.CreateAsync(dto, sellerId.Value);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var sellerId = GetCurrentUserId();
            if (sellerId == null)
                return Unauthorized();

            var product = await _productService.UpdateAsync(id, dto, sellerId.Value);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var sellerId = GetCurrentUserId();
            if (sellerId == null)
                return Unauthorized();

            await _productService.DeleteAsync(id, sellerId.Value);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(claim, out var id))
            return id;
        return null;
    }
}