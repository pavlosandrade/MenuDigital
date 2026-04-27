namespace MenuDigital.Domain.Entities.Tenants;

using MenuDigital.Domain.Shared;

/// <summary>
/// Representa o estabelecimento (Restaurante/Food Truck) no sistema.
/// Entidade Raiz de Agregação (Aggregate Root).
/// </summary>
public sealed class Tenant : EntityBase
{
    // Construtor vazio necessário para o ORM (EF Core) via Reflection
    private Tenant() {}

    private Tenant(string name, string slug) : base()
    {
        Name = name;
        Slug = slug;
        IsActive = true;
    }

    // Propriedades com set privado (imutabilidade externa)
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Subdomain { get; private set; }
    public bool IsActive { get; private set; }

    // Factory Method (Padrão Sênior para criação de entidades garantindo estado válido)
    public static Result<Tenant> Create(string name, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Tenant>(new Error("Tenant.NameRequired", "O nome do restaurante é obrigatório."));

        if (string.IsNullOrWhiteSpace(slug))
            return Result.Failure<Tenant>(new Error("Tenant.SlugRequired", "O identificador da URL (slug) é obrigatório."));

        if (slug.Contains(" "))
            return Result.Failure<Tenant>(new Error("Tenant.SlugInvalid", "O identificador da URL não pode conter espaços."));

        // Retorna o sucesso já com a entidade validada e populada
        return Result.Success(new Tenant(name, slug.ToLowerInvariant()));
    }

    // Comportamentos de Negócio (Rich Domain)
    public void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        UpdateTimestamp();
    }

    public Result SetSubdomain(string subdomain)
    {
        if (string.IsNullOrWhiteSpace(subdomain))
            return Result.Failure(new Error("Tenant.SubdomainEmpty", "O subdomínio não pode ser vazio."));

        Subdomain = subdomain.ToLowerInvariant();
        UpdateTimestamp();

        return Result.Success();
    }
}
