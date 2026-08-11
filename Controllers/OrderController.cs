using System.Security.Claims;
using DotNet_B2B_tradesphere.Extensions;
using DotNet_B2B_tradesphere.Services;
using DotNet_B2B_tradesphere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout()
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        if (!cart.Items.Any())
        {
            TempData["CartError"] = "Sepetiniz boş. Sipariş oluşturulamadı.";
            return RedirectToAction("Index", "Cart");
        }

        var dealerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(dealerId))
            return RedirectToAction("Login", "Auth");

        var orderId = await _orderService.CreateOrderAsync(cart, dealerId);
        if (orderId is null)
        {
            TempData["CartError"] = "Sipariş oluşturulamadı. Stok yetersiz olabilir.";
            return RedirectToAction("Index", "Cart");
        }

        HttpContext.Session.Remove(CartService.SessionKey);
        TempData["OrderId"] = orderId;
        return RedirectToAction(nameof(CheckoutSuccess));
    }

    [HttpGet]
    public IActionResult CheckoutSuccess()
    {
        ViewData["OrderId"] = TempData["OrderId"];
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var dealerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(dealerId))
            return RedirectToAction("Login", "Auth");

        var orders = await _orderService.GetOrdersByDealerAsync(dealerId);

        var viewModel = orders.Select(o => new OrderSummaryViewModel
        {
            OrderId = o.Id,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            TotalItems = o.OrderItems.Sum(i => i.Quantity)
        }).ToList();

        return View(viewModel);
    }
}
