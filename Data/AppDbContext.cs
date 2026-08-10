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
}
