using DotNet_B2B_tradesphere.Models;
using DotNet_B2B_tradesphere.Repositories;

namespace DotNet_B2B_tradesphere.Services;

public class DealerService : IDealerService
{
    private readonly IGenericRepository<Dealer> _dealerRepository;

    public DealerService(IGenericRepository<Dealer> dealerRepository)
    {
        _dealerRepository = dealerRepository;
    }

    public Task<Dealer?> GetByIdAsync(int id)
        => _dealerRepository.GetByIdAsync(id);

    public Task<IReadOnlyList<Dealer>> GetAllAsync()
        => _dealerRepository.GetAllAsync();

    public Task AddAsync(Dealer dealer)
        => _dealerRepository.AddAsync(dealer);

    public Task UpdateAsync(Dealer dealer)
        => _dealerRepository.UpdateAsync(dealer);

    public async Task DeleteAsync(int id)
    {
        var dealer = await _dealerRepository.GetByIdAsync(id);
        if (dealer is null)
            return;

        await _dealerRepository.DeleteAsync(dealer);
    }
}
