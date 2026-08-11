using System.ComponentModel.DataAnnotations;

namespace DotNet_B2B_tradesphere.ViewModels;

public class ProductCreateViewModel
{
    [Required(ErrorMessage = "Ürün adı zorunludur.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2-100 karakter arasında olmalıdır.")]
    [Display(Name = "Ürün Adı")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Fiyat zorunludur.")]
    [Range(0.01, 999999.99, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
    [Display(Name = "Fiyat")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stok miktarı zorunludur.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır.")]
    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }
}
