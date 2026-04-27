using MenuDigital.Domain.Entities.Categories;
using MenuDigital.Domain.Entities.Menus;
using MenuDigital.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuDigital.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(300);

        // Relacionamentos puramente baseados em DDD (sem propriedades de navegação bidirecionais poluindo a entidade)
        builder.HasOne<Tenant>()
               .WithMany()
               .HasForeignKey(c => c.TenantId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Menu>()
               .WithMany()
               .HasForeignKey(c => c.MenuId)
               .OnDelete(DeleteBehavior.Cascade); // Se o menu acabar, a categoria também vai
    }
}
