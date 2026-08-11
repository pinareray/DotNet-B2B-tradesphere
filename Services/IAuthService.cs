namespace DotNet_B2B_tradesphere.Services;

public record AuthResult(
    bool Success,
    int UserId = 0,
    string DisplayName = "",
    string Role = "",
    string TaxNumber = "");

public interface IAuthService
{
    Task<AuthResult> ValidateLoginAsync(string emailOrTaxNumber, string password);
}
