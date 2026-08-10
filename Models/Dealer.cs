namespace DotNet_B2B_tradesphere.Models;

public class Dealer : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public decimal DiscountRate { get; set; }
}
