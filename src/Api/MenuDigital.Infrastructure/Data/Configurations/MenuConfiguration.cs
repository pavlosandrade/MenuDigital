using MenuDigital.Domain.Entities.Menus;
using MenuDigital.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuDigital.Infrastructure.Data.Configurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("Menus");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
        
        // Mantém a precisão padrão de horário do SQL
        builder.Property(m => m.StartTime).HasColumnType("time");
        builder.Property(m => m.EndTime).HasColumnType("time");

        // Relacionamento forte com Tenant, impedindo deleção em cascata perigosa
        builder.HasOne<Tenant>() 
               .WithMany()
               .HasForeignKey(m => m.TenantId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
