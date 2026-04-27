using MenuDigital.Application.Interfaces;
using MenuDigital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MenuDigital.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException("DefaultConnection is missing in appsettings.");

        services.AddDbContext<MenuDigitalDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        // Abstrai o contexto para a camada Application injetar apenas a Interface
        services.AddScoped<IMenuDigitalDbContext>(provider => 
            provider.GetRequiredService<MenuDigitalDbContext>());

        return services;
    }
}
