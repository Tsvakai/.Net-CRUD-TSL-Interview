using Microsoft.AspNetCore.Mvc;
using RestApiExample.DTOs;
using RestApiExample.Exceptions;
using RestApiExample.Services;

namespace RestApiExample.Controllers;

/// <summary>
/// Controller for managing product resources.
/// </summary>
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a paginated list of products.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of products.</returns>
    /// <response code="200">Returns the list of products.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Retrieves a single product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The requested product.</returns>
    /// <response code="200">Returns the requested product.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="createDto">The product details to create.</param>
    /// <returns>The newly created product.</returns>
    /// <response code="201">Returns the newly created product.</response>
    /// <response code="400">If the validation fails.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="updateDto">The updated product details.</param>
    /// <returns>The updated product.</returns>
    /// <response code="200">Returns the updated product.</response>
    /// <response code="400">If the validation fails.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <response code="204">If the deletion is successful.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
