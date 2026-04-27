using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Entities.Categories;
using MenuDigital.Domain.Entities.Menus;
using MenuDigital.Domain.Entities.Products;
using MenuDigital.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Infrastructure.Data;

public class MenuDigitalDbContext : DbContext, IMenuDigitalDbContext
{
    private readonly ITenantService _tenantService;

    public MenuDigitalDbContext(DbContextOptions<MenuDigitalDbContext> options, ITenantService tenantService) 
        : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<RestaurantSettings> RestaurantSettings => Set<RestaurantSettings>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductAddon> ProductAddons => Set<ProductAddon>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automaticamente todos os IEntityTypeConfiguration<T> (FluentAPI)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MenuDigitalDbContext).Assembly);

        // ==========================================
        // ISOLAMENTO MULTI-TENANT (Global Query Filter)
        // ==========================================
        // Essas regras garantem que desenvolvedores não vazem dados de outro restaurante por acidente.
        // Aplicamos o filtro em todas as entidades atreladas a um Tenant.
        // O lambda DEVE chamar _tenantService.GetTenantId() diretamente para ser dinâmico por request.
        modelBuilder.Entity<RestaurantSettings>().HasQueryFilter(e => e.TenantId == _tenantService.GetTenantId());
        modelBuilder.Entity<Menu>().HasQueryFilter(e => e.TenantId == _tenantService.GetTenantId());
        modelBuilder.Entity<Category>().HasQueryFilter(e => e.TenantId == _tenantService.GetTenantId());
        modelBuilder.Entity<Product>().HasQueryFilter(e => e.TenantId == _tenantService.GetTenantId());
        modelBuilder.Entity<ProductAddon>().HasQueryFilter(e => e.TenantId == _tenantService.GetTenantId());
    }
}
