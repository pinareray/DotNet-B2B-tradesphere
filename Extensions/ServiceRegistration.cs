using DotNet_B2B_tradesphere.Data;
using DotNet_B2B_tradesphere.Repositories;
using DotNet_B2B_tradesphere.Services;
using Microsoft.EntityFrameworkCore;

namespace DotNet_B2B_tradesphere.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IDealerService, DealerService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
