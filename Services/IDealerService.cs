using DotNet_B2B_tradesphere.Models;

namespace DotNet_B2B_tradesphere.Services;

public interface IDealerService
{
    Task<Dealer?> GetByIdAsync(int id);
    Task<IReadOnlyList<Dealer>> GetAllAsync();
    Task AddAsync(Dealer dealer);
    Task UpdateAsync(Dealer dealer);
    Task DeleteAsync(int id);
}
