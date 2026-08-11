using DotNet_B2B_tradesphere.Models;
using DotNet_B2B_tradesphere.Services;
using DotNet_B2B_tradesphere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_B2B_tradesphere.Controllers;

[Authorize]
public class ProductController : BaseController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(bool inStockOnly = false)
    {
        var products = inStockOnly
            ? await _productService.GetInStockProductsAsync()
            : await _productService.GetAllActiveAsync();

        var viewModel = products.Select(p => new ProductListViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            IsActive = p.IsActive
        }).ToList();

        ViewData["InStockOnly"] = inStockOnly;
        return View(viewModel);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet]
    public IActionResult Create()
        => View(new ProductCreateViewModel());

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var product = new Product
        {
            Name = model.Name,
            Price = model.Price,
            StockQuantity = model.StockQuantity
        };

        await _productService.AddAsync(product);
        ShowAlert("Başarılı", "Ürün başarıyla eklendi.", "success");
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
            return NotFound();

        var model = new ProductUpdateViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };

        return View(model);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductUpdateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var product = await _productService.GetByIdAsync(model.Id);
        if (product is null)
            return NotFound();

        product.Name = model.Name;
        product.Price = model.Price;
        product.StockQuantity = model.StockQuantity;
        product.IsActive = model.IsActive;

        await _productService.UpdateAsync(product);
        ShowAlert("Başarılı", "Ürün başarıyla güncellendi.", "success");
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        ShowAlert("Başarılı", "Ürün başarıyla silindi.", "success");
        return RedirectToAction(nameof(Index));
    }
}
