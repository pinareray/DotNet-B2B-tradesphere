using DotNet_B2B_tradesphere.Models;
using DotNet_B2B_tradesphere.ViewModels;

namespace DotNet_B2B_tradesphere.Services;

public interface IOrderService
{
    Task<int?> CreateOrderAsync(CartViewModel cart, string dealerId);
    Task<IReadOnlyList<Order>> GetOrdersByDealerAsync(string dealerId);
}
