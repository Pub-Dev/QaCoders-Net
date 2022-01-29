using QaCoders_Net.Entities;
using QaCoders_Net.Interfaces.Repositories;
using QaCoders_Net.Interfaces.Services;

namespace QaCoders_Net.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> GetByIdAsync(int productId)
    {
        return await _productRepository.GetByIdAsync(productId);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        return await _productRepository.CreateAsync(product);
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        return await _productRepository.UpdateAsync(product);
    }
}
