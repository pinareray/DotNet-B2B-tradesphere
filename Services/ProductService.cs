using DotNet_B2B_tradesphere.Models;
using DotNet_B2B_tradesphere.Repositories;

namespace DotNet_B2B_tradesphere.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Dealer> _dealerRepository;

    public ProductService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<Dealer> dealerRepository)
    {
        _productRepository = productRepository;
        _dealerRepository = dealerRepository;
    }

    public Task<IReadOnlyList<Product>> GetAllActiveAsync()
        => _productRepository.FindAsync(p => p.IsActive);

    public Task<Product?> GetByIdAsync(int id)
        => _productRepository.GetByIdAsync(id);

    public Task<IReadOnlyList<Product>> GetInStockProductsAsync()
        => _productRepository.FindAsync(p => p.IsActive && p.StockQuantity > 0);

    public async Task<IReadOnlyList<Product>> GetProductsForDealerAsync(int dealerId)
    {
        var dealer = await _dealerRepository.GetByIdAsync(dealerId);
        if (dealer is null || !dealer.IsActive)
            return Array.Empty<Product>();

        return await _productRepository.FindAsync(p => p.IsActive && p.StockQuantity > 0);
    }

    public Task AddAsync(Product product)
        => _productRepository.AddAsync(product);

    public Task UpdateAsync(Product product)
        => _productRepository.UpdateAsync(product);

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            return;

        await _productRepository.DeleteAsync(product);
    }
}
