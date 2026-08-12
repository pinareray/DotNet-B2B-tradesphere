using DotNet_B2B_tradesphere.Data;
using DotNet_B2B_tradesphere.Models;
using DotNet_B2B_tradesphere.ViewModels;
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
            Email: dealer.Email,
            Role: dealer.Role,
            TaxNumber: dealer.TaxNumber);
    }

    public async Task<RegisterResult> RegisterDealerAsync(RegisterViewModel model)
    {
        var email = model.Email.Trim();
        var taxNumber = model.TaxNumber.Trim();

        var exists = await _context.Dealers.AnyAsync(d =>
            d.Email == email || d.TaxNumber == taxNumber);

        if (exists)
            return new RegisterResult(false, "Bu e-posta veya vergi numarası ile kayıtlı bir bayi zaten var.");

        var dealer = new Dealer
        {
            CompanyName = model.CompanyName.Trim(),
            TaxNumber = taxNumber,
            Email = email,
            Role = AppRoles.Dealer,
            DiscountRate = 0,
            CreatedDate = DateTime.UtcNow,
            IsActive = true
        };

        dealer.PasswordHash = _passwordHasher.HashPassword(dealer, model.Password);

        _context.Dealers.Add(dealer);
        await _context.SaveChangesAsync();

        return new RegisterResult(true);
    }
}
