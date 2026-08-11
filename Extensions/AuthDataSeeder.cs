using DotNet_B2B_tradesphere.Data;
using DotNet_B2B_tradesphere.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotNet_B2B_tradesphere.Extensions;

public static class AuthDataSeeder
{
    private const string DefaultPassword = "Dealer123!";

    public static async Task SeedDealerCredentialsAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var hasher = new PasswordHasher<Dealer>();

        var dealers = await context.Dealers
            .Where(d => string.IsNullOrEmpty(d.PasswordHash))
            .ToListAsync();

        foreach (var dealer in dealers)
        {
            if (string.IsNullOrEmpty(dealer.Email))
            {
                dealer.Email = dealer.TaxNumber == "1234567890"
                    ? "abc@tradesphere.com"
                    : "xyz@tradesphere.com";
            }

            if (string.IsNullOrEmpty(dealer.Role))
                dealer.Role = AppRoles.Dealer;

            dealer.PasswordHash = hasher.HashPassword(dealer, DefaultPassword);
        }

        if (dealers.Count > 0)
            await context.SaveChangesAsync();
    }
}
