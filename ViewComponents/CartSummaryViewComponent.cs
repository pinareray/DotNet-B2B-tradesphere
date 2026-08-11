using DotNet_B2B_tradesphere.Extensions;
using DotNet_B2B_tradesphere.Services;
using DotNet_B2B_tradesphere.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.ViewComponents;

public class CartSummaryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session.GetJson<CartViewModel>(CartService.SessionKey)
                   ?? new CartViewModel();

        return View(cart.TotalItems);
    }
}
