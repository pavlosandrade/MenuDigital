namespace MenuDigital.Application.Menus.Queries.GetMenuCatalog;

// Agrupamos os DTOs de leitura do catálogo. Eles são "achatados" e só contêm os dados que o cliente precisa ler.
// Utilizamos 'record' por serem estruturas de dados imutáveis, perfeitas para leitura (Read-Model do CQRS).

public record ProductAddonDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public bool IsRequired { get; init; }
    public int MaxSelection { get; init; }
    public int DisplayOrder { get; init; }
}

public record ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsAvailable { get; init; }
    public IEnumerable<ProductAddonDto> Addons { get; init; } = Array.Empty<ProductAddonDto>();
}

public record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int DisplayOrder { get; init; }
    public IEnumerable<ProductDto> Products { get; init; } = Array.Empty<ProductDto>();
}

public record MenuCatalogDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IEnumerable<CategoryDto> Categories { get; init; } = Array.Empty<CategoryDto>();
}
