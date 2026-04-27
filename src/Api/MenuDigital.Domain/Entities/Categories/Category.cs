namespace MenuDigital.Domain.Entities.Categories;

using MenuDigital.Domain.Shared;

public sealed class Category : EntityBase
{
    private Category() {}

    private Category(Guid tenantId, Guid menuId, string name, string? description, int displayOrder) : base()
    {
        TenantId = tenantId;
        MenuId = menuId;
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        IsActive = true;
    }

    public Guid TenantId { get; private set; }
    public Guid MenuId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }

    public static Result<Category> Create(Guid tenantId, Guid menuId, string name, string? description, int displayOrder)
    {
        if (tenantId == Guid.Empty)
            return Result.Failure<Category>(new Error("Category.TenantInvalid", "O ID do restaurante é inválido."));

        if (menuId == Guid.Empty)
            return Result.Failure<Category>(new Error("Category.MenuInvalid", "O ID do cardápio é inválido."));

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Category>(new Error("Category.NameRequired", "O nome da categoria é obrigatório."));

        if (displayOrder < 0)
            return Result.Failure<Category>(new Error("Category.InvalidOrder", "A ordem de exibição não pode ser negativa."));

        return Result.Success(new Category(tenantId, menuId, name, description, displayOrder));
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public void UpdateDisplayOrder(int newOrder)
    {
        if (newOrder < 0) return;
        DisplayOrder = newOrder;
        UpdateTimestamp();
    }
}
