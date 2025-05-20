using RestApiExample.DTOs;

namespace RestApiExample.Services;

public interface IProductService
{
    Task<(IEnumerable<ProductDto> Products, int TotalCount)> GetAllProductsAsync(int pageNumber, int pageSize);
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task<bool> DeleteProductAsync(int id);
}
