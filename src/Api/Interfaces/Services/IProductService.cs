using QaCoders_Net.Entities;

namespace QaCoders_Net.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<Product> GetByIdAsync(int productId);

    Task<Product> CreateAsync(Product product);

    Task<Product> UpdateAsync(Product product);
}