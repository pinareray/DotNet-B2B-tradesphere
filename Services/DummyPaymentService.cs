using DotNet_B2B_tradesphere.ViewModels;

namespace DotNet_B2B_tradesphere.Services;

public class DummyPaymentService : IPaymentService
{
    public Task<bool> ProcessPaymentAsync(PaymentViewModel model)
    {
        var cardNumber = model.CardNumber
            .Replace(" ", string.Empty)
            .Replace("-", string.Empty);

        if (cardNumber.StartsWith("0000"))
            return Task.FromResult(false);

        return Task.FromResult(true);
    }
}
