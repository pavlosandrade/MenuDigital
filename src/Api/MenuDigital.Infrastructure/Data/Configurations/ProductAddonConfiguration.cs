using MenuDigital.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuDigital.Infrastructure.Data.Configurations;

public class ProductAddonConfiguration : IEntityTypeConfiguration<ProductAddon>
{
    public void Configure(EntityTypeBuilder<ProductAddon> builder)
    {
        builder.ToTable("ProductAddons");
        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.Name).IsRequired().HasMaxLength(100);
        builder.Property(pa => pa.Description).HasMaxLength(300);
        builder.Property(pa => pa.Price).HasColumnType("decimal(18,2)");

        // A FK já foi configurada no ProductConfiguration via HasMany
    }
}
