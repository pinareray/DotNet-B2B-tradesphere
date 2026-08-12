using DotNet_B2B_tradesphere.Models;

namespace DotNet_B2B_tradesphere.Services;

public interface IInvoiceService
{
    byte[] GenerateInvoicePdf(Order order, Dealer dealer);
}
