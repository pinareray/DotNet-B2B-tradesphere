using DotNet_B2B_tradesphere.ViewModels;

namespace DotNet_B2B_tradesphere.Services;

public record AuthResult(
    bool Success,
    int UserId = 0,
    string DisplayName = "",
    string Email = "",
    string Role = "",
    string TaxNumber = "");

public record RegisterResult(bool Success, string ErrorMessage = "");

public interface IAuthService
{
    Task<AuthResult> ValidateLoginAsync(string emailOrTaxNumber, string password);
    Task<RegisterResult> RegisterDealerAsync(RegisterViewModel model);
}
