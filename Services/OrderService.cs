using DotNet_B2B_tradesphere.Data;
using DotNet_B2B_tradesphere.Models;
using DotNet_B2B_tradesphere.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet_B2B_tradesphere.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int?> CreateOrderAsync(CartViewModel cart, string dealerId)
    {
        if (cart.Items.Count == 0 || !int.TryParse(dealerId, out var dealerIdInt))
            return null;

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var order = new Order
            {
                DealerId = dealerIdInt,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.TotalAmount,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product is null || !product.IsActive || product.StockQuantity < item.Quantity)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                product.StockQuantity -= item.Quantity;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IReadOnlyList<Order>> GetOrdersByDealerAsync(string dealerId)
    {
        if (!int.TryParse(dealerId, out var dealerIdInt))
            return Array.Empty<Order>();

        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .Where(o => o.DealerId == dealerIdInt)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Include(o => o.Dealer)
            .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}
