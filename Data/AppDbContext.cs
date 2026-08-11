using DotNet_B2B_tradesphere.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet_B2B_tradesphere.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Dealer> Dealers => Set<Dealer>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Dealer>().HasData(
            new Dealer
            {
                Id = 1,
                CompanyName = "ABC Teknoloji Ltd.",
                TaxNumber = "1234567890",
                DiscountRate = 15.0m,
                CreatedDate = seedDate,
                IsActive = true
            },
            new Dealer
            {
                Id = 2,
                CompanyName = "XYZ Ticaret A.Ş.",
                TaxNumber = "9876543210",
                DiscountRate = 10.0m,
                CreatedDate = seedDate,
                IsActive = true
            });
    }
}
