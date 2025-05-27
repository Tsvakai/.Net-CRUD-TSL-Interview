using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RestApiExample.DTOs;
using RestApiExample.Models;
using RestApiExample.Repositories;
using RestApiExample.Exceptions;

namespace RestApiExample.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, IMapper mapper, ILogger<ProductService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<(IEnumerable<ProductDto> Products, int TotalCount)> GetAllProductsAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Retrieving products (Page: {PageNumber}, Size: {PageSize})", pageNumber, pageSize);

        if (pageNumber < 1 || pageSize < 1)
        {
            throw new ValidationException("Page number and size must be greater than zero.");
        }

        try
        {
            var (products, totalCount) = await _repository.GetAllAsync(pageNumber, pageSize);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products) ?? Enumerable.Empty<ProductDto>();

            return (productDtos, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all products.");
            throw new ServiceException("Failed to retrieve products.", ex);
        }
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving product by ID: {ProductId}", id);

        if (id <= 0)
        {
            throw new ValidationException("Product ID must be greater than zero.");
        }

        try
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found.");
            }

            return _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "Error occurred while retrieving product with ID: {ProductId}", id);
            throw new ServiceException($"Failed to retrieve product with ID {id}.", ex);
        }
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
{
    _logger.LogInformation("Creating a new product");

    if (createProductDto == null)
    {
        throw new ValidationException("Product data must be provided.");
    }

    try
    {
        // Check for duplicate product name
        var existingProduct = await _repository.GetByNameAsync(createProductDto.Name);
        if (existingProduct != null)
        {
            throw new ValidationException($"Product with name '{createProductDto.Name}' already exists.");
        }

        var product = _mapper.Map<Product>(createProductDto);
        var createdProduct = await _repository.CreateAsync(product);
        return _mapper.Map<ProductDto>(createdProduct);
    }
    catch (Exception ex) when (ex is not ValidationException)
    {
        _logger.LogError(ex, "Error occurred while creating a product.");
        throw new ServiceException("Failed to create product.", ex);
    }
}


    public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);

        if (id <= 0)
        {
            throw new ValidationException("Product ID must be greater than zero.");
        }

        if (updateProductDto == null)
        {
            throw new ValidationException("Update data must be provided.");
        }

        try
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found.");
            }

            _mapper.Map(updateProductDto, product);
            await _repository.UpdateAsync(product);

            return _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "Error occurred while updating product with ID: {ProductId}", id);
            throw new ServiceException($"Failed to update product with ID {id}.", ex);
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);

        if (id <= 0)
        {
            throw new ValidationException("Product ID must be greater than zero.");
        }

        try
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with ID {id} not found.");
            }

            await _repository.DeleteAsync(product);
            return true;
        }
        catch (Exception ex) when (ex is not NotFoundException)
        {
            _logger.LogError(ex, "Error occurred while deleting product with ID: {ProductId}", id);
            throw new ServiceException($"Failed to delete product with ID {id}.", ex);
        }
    }
}
