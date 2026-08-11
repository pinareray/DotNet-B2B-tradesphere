namespace DotNet_B2B_tradesphere.Models;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public int DealerId { get; set; }
    public Dealer Dealer { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
