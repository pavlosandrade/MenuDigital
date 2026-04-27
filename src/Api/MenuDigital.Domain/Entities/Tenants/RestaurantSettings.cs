namespace MenuDigital.Domain.Entities.Tenants;

using MenuDigital.Domain.Shared;

public sealed class RestaurantSettings : EntityBase
{
    private RestaurantSettings() {}

    private RestaurantSettings(Guid tenantId) : base()
    {
        TenantId = tenantId;
        PrimaryColor = "#FF5A5F"; // Cor base padrão
        SecondaryColor = "#2B2B2B";
        IsActive = true;
    }

    public Guid TenantId { get; private set; }
    
    // Theme
    public string PrimaryColor { get; private set; } = string.Empty;
    public string SecondaryColor { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public string? BannerUrl { get; private set; }

    // Contact Info
    public string? Phone { get; private set; }
    public string? WhatsApp { get; private set; }
    public string? Address { get; private set; }
    public string? GoogleMapsUrl { get; private set; }

    // Social Links
    public string? InstagramUrl { get; private set; }
    public string? FacebookUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }

    public bool IsActive { get; private set; }

    public static Result<RestaurantSettings> CreateDefault(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            return Result.Failure<RestaurantSettings>(new Error("RestaurantSettings.TenantInvalid", "O ID do restaurante é inválido."));

        return Result.Success(new RestaurantSettings(tenantId));
    }

    public Result UpdateTheme(string primaryColor, string secondaryColor, string? logoUrl, string? bannerUrl)
    {
        if (string.IsNullOrWhiteSpace(primaryColor) || string.IsNullOrWhiteSpace(secondaryColor))
            return Result.Failure(new Error("Theme.ColorsRequired", "As cores primária e secundária são obrigatórias."));

        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
        LogoUrl = logoUrl;
        BannerUrl = bannerUrl;
        
        UpdateTimestamp();
        return Result.Success();
    }

    public void UpdateContactInfo(string? phone, string? whatsapp, string? address, string? mapsUrl)
    {
        Phone = phone;
        WhatsApp = whatsapp;
        Address = address;
        GoogleMapsUrl = mapsUrl;
        UpdateTimestamp();
    }
}
