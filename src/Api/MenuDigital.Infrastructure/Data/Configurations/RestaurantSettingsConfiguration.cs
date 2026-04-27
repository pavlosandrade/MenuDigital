using MenuDigital.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuDigital.Infrastructure.Data.Configurations;

public class RestaurantSettingsConfiguration : IEntityTypeConfiguration<RestaurantSettings>
{
    public void Configure(EntityTypeBuilder<RestaurantSettings> builder)
    {
        builder.ToTable("RestaurantSettings");
        builder.HasKey(rs => rs.Id);

        // HEX colors ex: #FF00FF
        builder.Property(rs => rs.PrimaryColor).IsRequired().HasMaxLength(7); 
        builder.Property(rs => rs.SecondaryColor).IsRequired().HasMaxLength(7);
        builder.Property(rs => rs.LogoUrl).HasMaxLength(1000);
        builder.Property(rs => rs.BannerUrl).HasMaxLength(1000);

        builder.Property(rs => rs.Phone).HasMaxLength(20);
        builder.Property(rs => rs.WhatsApp).HasMaxLength(20);
        builder.Property(rs => rs.Address).HasMaxLength(500);
        builder.Property(rs => rs.GoogleMapsUrl).HasMaxLength(1000);

        builder.Property(rs => rs.InstagramUrl).HasMaxLength(500);
        builder.Property(rs => rs.FacebookUrl).HasMaxLength(500);
        builder.Property(rs => rs.WebsiteUrl).HasMaxLength(500);

        // Restrição 1-1
        builder.HasOne<Tenant>()
               .WithOne()
               .HasForeignKey<RestaurantSettings>(rs => rs.TenantId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
