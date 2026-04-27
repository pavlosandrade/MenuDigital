using MenuDigital.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuDigital.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
        
        // Padrão financeiro (18 digitos, 2 casas)
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)"); 

        builder.Property(p => p.ImageUrl).HasMaxLength(1000);

        // Relacionamento forte com a coleção de adicionais (Composition)
        builder.HasMany(p => p.Addons)
               .WithOne()
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade); // Se apagar o produto, apaga os adicionais

        // HasAddons é calculada, não precisa ir pro banco
        builder.Ignore(p => p.HasAddons);
    }
}
