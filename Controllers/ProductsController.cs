using Microsoft.AspNetCore.Mvc;
using RestApiExample.DTOs;
using RestApiExample.Services;
using RestApiExample.Exceptions;

namespace RestApiExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: api/products?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var (products, totalCount) = await _productService.GetAllProductsAsync(pageNumber, pageSize);
        return Ok(new
        {
            data = products,
            totalCount,
            pageNumber,
            pageSize
        });
    }

    // GET: api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Product with ID {ProductId} not found", id);
            return NotFound(new { message = ex.Message });
        }
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var product = await _productService.CreateProductAsync(createDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed during product creation.");
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/products/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var updated = await _productService.UpdateProductAsync(id, updateDto);
            return Ok(updated);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Product with ID {ProductId} not found for update.", id);
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed while updating product with ID {ProductId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var deleted = await _productService.DeleteProductAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Product with ID {ProductId} not found for deletion.", id);
            return NotFound(new { message = ex.Message });
        }
    }
}
