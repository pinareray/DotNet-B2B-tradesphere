namespace DotNet_B2B_tradesphere.ViewModels;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);

    public int TotalItems => Items.Sum(i => i.Quantity);
}
