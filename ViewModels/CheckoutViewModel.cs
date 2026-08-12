namespace DotNet_B2B_tradesphere.ViewModels;

public class CheckoutViewModel
{
    public PaymentViewModel Payment { get; set; } = new();
    public CartViewModel Cart { get; set; } = new();
}
