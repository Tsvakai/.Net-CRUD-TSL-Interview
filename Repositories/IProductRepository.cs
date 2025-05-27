using RestApiExample.Models;

namespace RestApiExample.Repositories
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product>, int)> GetAllAsync(int pageNumber, int pageSize);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<Product?> GetByNameAsync(string name);

    }
}
