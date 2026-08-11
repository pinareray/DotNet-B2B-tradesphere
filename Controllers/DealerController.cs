using DotNet_B2B_tradesphere.Services;
using DotNet_B2B_tradesphere.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.Controllers;

public class DealerController : Controller
{
    private readonly IDealerService _dealerService;

    public DealerController(IDealerService dealerService)
    {
        _dealerService = dealerService;
    }

    public async Task<IActionResult> Index()
    {
        var dealers = await _dealerService.GetAllAsync();

        var viewModel = dealers.Select(d => new DealerListViewModel
        {
            Id = d.Id,
            CompanyName = d.CompanyName,
            TaxNumber = d.TaxNumber,
            DiscountRate = d.DiscountRate
        }).ToList();

        return View(viewModel);
    }
}
