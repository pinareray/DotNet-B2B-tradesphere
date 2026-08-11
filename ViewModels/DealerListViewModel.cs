namespace DotNet_B2B_tradesphere.ViewModels;

public class DealerListViewModel
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public decimal DiscountRate { get; set; }
}
