using DotNet_B2B_tradesphere.Data;
using DotNet_B2B_tradesphere.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNet_B2B_tradesphere.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<Dealer> _passwordHasher = new();

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AuthResult> ValidateLoginAsync(string emailOrTaxNumber, string password)
    {
        var identifier = emailOrTaxNumber.Trim();

        var dealer = await _context.Dealers
            .AsNoTracking()
            .FirstOrDefaultAsync(d =>
                d.IsActive &&
                (d.Email == identifier || d.TaxNumber == identifier));

        if (dealer is null || string.IsNullOrEmpty(dealer.PasswordHash))
            return new AuthResult(Success: false);

        var verification = _passwordHasher.VerifyHashedPassword(dealer, dealer.PasswordHash, password);
        if (verification == PasswordVerificationResult.Failed)
            return new AuthResult(Success: false);

        return new AuthResult(
            Success: true,
            UserId: dealer.Id,
            DisplayName: dealer.CompanyName,
            Role: dealer.Role,
            TaxNumber: dealer.TaxNumber);
    }
}
