using System.Security.Claims;
using DotNet_B2B_tradesphere.Hubs;
using DotNet_B2B_tradesphere.Services;
using DotNet_B2B_tradesphere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DotNet_B2B_tradesphere.Controllers;

[Authorize]
public class OrderController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly IPaymentService _paymentService;
    private readonly IHubContext<OrderHub> _hubContext;

    public OrderController(
        IOrderService orderService,
        ICartService cartService,
        IPaymentService paymentService,
        IHubContext<OrderHub> hubContext)
    {
        _orderService = orderService;
        _cartService = cartService;
        _paymentService = paymentService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        if (!cart.Items.Any())
        {
            ShowAlert("Hata", "Sepetiniz boş. Ödeme sayfasına geçilemedi.", "error");
            return RedirectToAction("Index", "Cart");
        }

        return View(new CheckoutViewModel { Cart = cart });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        if (!cart.Items.Any())
        {
            ShowAlert("Hata", "Sepetiniz boş. Sipariş oluşturulamadı.", "error");
            return RedirectToAction("Index", "Cart");
        }

        model.Cart = cart;

        if (!ModelState.IsValid)
            return View(model);

        var paymentSuccess = await _paymentService.ProcessPaymentAsync(model.Payment);
        if (!paymentSuccess)
        {
            ModelState.AddModelError(string.Empty, "Ödeme reddedildi. Lütfen kart bilgilerinizi kontrol edin.");
            ShowAlert("Ödeme Reddedildi", "Sanal POS işlemi başarısız oldu. Kart numaranızı kontrol edin.", "error");
            return View(model);
        }

        var dealerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(dealerId))
            return RedirectToAction("Login", "Auth");

        var orderId = await _orderService.CreateOrderAsync(cart, dealerId);
        if (orderId is null)
        {
            ShowAlert("Hata", "Sipariş oluşturulamadı. Stok yetersiz olabilir.", "error");
            return RedirectToAction("Index", "Cart");
        }

        _cartService.ClearCart(HttpContext.Session);
        TempData["OrderId"] = orderId;

        await _hubContext.Clients.All.SendAsync(
            "ReceiveNewOrder",
            "Yeni bir sipariş alındı! Sipariş Tutarı: " + cart.TotalAmount.ToString("C2"));

        ShowAlert("Sipariş Alındı", $"Ödemeniz alındı. Sipariş No: #{orderId}", "success");
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
