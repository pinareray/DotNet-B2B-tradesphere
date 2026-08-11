using DotNet_B2B_tradesphere.Models;

namespace DotNet_B2B_tradesphere.Services;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetAllActiveAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetInStockProductsAsync();
    Task<IReadOnlyList<Product>> GetProductsForDealerAsync(int dealerId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
