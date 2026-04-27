using MenuDigital.Domain.Entities.Categories;
using MenuDigital.Domain.Entities.Menus;
using MenuDigital.Domain.Entities.Products;
using MenuDigital.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Application.Interfaces;

public interface IMenuDigitalDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<RestaurantSettings> RestaurantSettings { get; }
    DbSet<Menu> Menus { get; }
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    DbSet<ProductAddon> ProductAddons { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
