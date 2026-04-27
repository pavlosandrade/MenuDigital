namespace MenuDigital.Domain.Entities.Products;

using MenuDigital.Domain.Shared;

public sealed class Product : EntityBase
{
    private readonly List<ProductAddon> _addons = new();

    private Product() {}

    private Product(Guid tenantId, Guid categoryId, string name, string? description, decimal price, string? imageUrl, int displayOrder) : base()
    {
        TenantId = tenantId;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
        DisplayOrder = displayOrder;
        IsAvailable = true;
    }

    public Guid TenantId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public bool IsAvailable { get; private set; }
    public int DisplayOrder { get; private set; }

    // Propriedade calculada baseada na lista de adicionais
    public bool HasAddons => _addons.Any();
    
    // O IReadOnlyCollection garante que ninguém dê um .Add() de fora da classe
    public IReadOnlyCollection<ProductAddon> Addons => _addons.AsReadOnly();

    public static Result<Product> Create(Guid tenantId, Guid categoryId, string name, string? description, decimal price, string? imageUrl, int displayOrder)
    {
        if (tenantId == Guid.Empty) return Result.Failure<Product>(new Error("Product.TenantInvalid", "O ID do restaurante é inválido."));
        if (categoryId == Guid.Empty) return Result.Failure<Product>(new Error("Product.CategoryInvalid", "O ID da categoria é inválido."));
        if (string.IsNullOrWhiteSpace(name)) return Result.Failure<Product>(new Error("Product.NameRequired", "O nome do produto é obrigatório."));
        if (price < 0) return Result.Failure<Product>(new Error("Product.InvalidPrice", "O preço não pode ser negativo."));

        return Result.Success(new Product(tenantId, categoryId, name, description, price, imageUrl, displayOrder));
    }

    public void MarkAsUnavailable()
    {
        IsAvailable = false;
        UpdateTimestamp();
    }

    public void MarkAsAvailable()
    {
        IsAvailable = true;
        UpdateTimestamp();
    }

    public Result AddAddon(string name, string? description, decimal price, bool isRequired, int maxSelection, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(new Error("ProductAddon.NameRequired", "O nome do adicional é obrigatório."));
            
        if (price < 0)
            return Result.Failure(new Error("ProductAddon.InvalidPrice", "O preço do adicional não pode ser negativo."));

        if (maxSelection < 1 && !isRequired)
            return Result.Failure(new Error("ProductAddon.InvalidMaxSelection", "A seleção máxima deve ser ao menos 1."));

        var addon = new ProductAddon(TenantId, Id, name, description, price, isRequired, maxSelection, displayOrder);
        _addons.Add(addon);
        
        UpdateTimestamp();
        return Result.Success();
    }
}
