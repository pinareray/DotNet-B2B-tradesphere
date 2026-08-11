using DotNet_B2B_tradesphere.Extensions;
using DotNet_B2B_tradesphere.ViewModels;

namespace DotNet_B2B_tradesphere.Services;

public class CartService : ICartService
{
    public const string SessionKey = "Cart";

    private readonly IProductService _productService;

    public CartService(IProductService productService)
    {
        _productService = productService;
    }

    public CartViewModel GetCart(ISession session)
        => session.GetJson<CartViewModel>(SessionKey) ?? new CartViewModel();

    public async Task<bool> AddToCartAsync(ISession session, int productId, int quantity = 1)
    {
        if (quantity < 1)
            return false;

        var product = await _productService.GetByIdAsync(productId);
        if (product is null || !product.IsActive || product.StockQuantity <= 0)
            return false;

        var cart = GetCart(session);
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem is not null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            if (newQuantity > product.StockQuantity)
                return false;

            existingItem.Quantity = newQuantity;
            existingItem.Price = product.Price;
            existingItem.ProductName = product.Name;
        }
        else
        {
            if (quantity > product.StockQuantity)
                return false;

            cart.Items.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            });
        }

        session.SetJson(SessionKey, cart);
        return true;
    }

    public void RemoveFromCart(ISession session, int productId)
    {
        var cart = GetCart(session);
        cart.Items.RemoveAll(i => i.ProductId == productId);
        session.SetJson(SessionKey, cart);
    }

    public void ClearCart(ISession session)
        => session.Remove(SessionKey);

    public int GetItemCount(ISession session)
        => GetCart(session).TotalItems;
}
