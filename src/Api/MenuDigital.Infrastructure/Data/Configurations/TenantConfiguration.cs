using MenuDigital.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuDigital.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(150);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(100);
        
        // Slug único para não termos dois restaurantes com a mesma URL /r/nome
        builder.HasIndex(t => t.Slug).IsUnique(); 

        builder.Property(t => t.Subdomain).HasMaxLength(100);
    }
}
