namespace DotNet_B2B_tradesphere.Models;

public class Dealer : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = AppRoles.Dealer;
    public decimal DiscountRate { get; set; }
}
