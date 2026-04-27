namespace MenuDigital.Domain.Entities.Products;

using MenuDigital.Domain.Shared;

public sealed class ProductAddon : EntityBase
{
    private ProductAddon() {}

    internal ProductAddon(Guid tenantId, Guid productId, string name, string? description, decimal price, bool isRequired, int maxSelection, int displayOrder) : base()
    {
        TenantId = tenantId;
        ProductId = productId;
        Name = name;
        Description = description;
        Price = price;
        IsRequired = isRequired;
        MaxSelection = maxSelection;
        DisplayOrder = displayOrder;
        IsAvailable = true;
    }

    public Guid TenantId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public bool IsRequired { get; private set; }
    public int MaxSelection { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsAvailable { get; private set; }

    public void ToggleAvailability()
    {
        IsAvailable = !IsAvailable;
        UpdateTimestamp();
    }
}
