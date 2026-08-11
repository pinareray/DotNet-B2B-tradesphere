using DotNet_B2B_tradesphere.ViewModels;

namespace DotNet_B2B_tradesphere.Services;

public interface ICartService
{
    CartViewModel GetCart(ISession session);
    Task<bool> AddToCartAsync(ISession session, int productId, int quantity = 1);
    void RemoveFromCart(ISession session, int productId);
    void ClearCart(ISession session);
    int GetItemCount(ISession session);
}
