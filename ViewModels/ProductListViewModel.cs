namespace DotNet_B2B_tradesphere.ViewModels;

public class ProductListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public string StockStatus => StockQuantity > 0 ? "Stokta" : "Tükendi";
}
