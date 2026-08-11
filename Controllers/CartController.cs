using DotNet_B2B_tradesphere.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var added = await _cartService.AddToCartAsync(HttpContext.Session, productId, quantity);
        if (!added)
            TempData["CartError"] = "Ürün sepete eklenemedi. Stok yetersiz veya ürün bulunamadı.";

        return RedirectToAction("Index", "Product");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart(int productId)
    {
        _cartService.RemoveFromCart(HttpContext.Session, productId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearCart()
    {
        _cartService.ClearCart(HttpContext.Session);
        return RedirectToAction(nameof(Index));
    }
}
