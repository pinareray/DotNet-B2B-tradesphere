using DotNet_B2B_tradesphere.ViewModels;

namespace DotNet_B2B_tradesphere.Services;

public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(PaymentViewModel model);
}
