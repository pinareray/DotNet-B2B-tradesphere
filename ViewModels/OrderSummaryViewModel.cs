namespace DotNet_B2B_tradesphere.ViewModels;

public class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalItems { get; set; }
}
