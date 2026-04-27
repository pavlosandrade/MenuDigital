using MediatR;
using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Application.Products.Commands.AddProductAddon;

public sealed class AddProductAddonCommandHandler : IRequestHandler<AddProductAddonCommand, Result>
{
    private readonly IMenuDigitalDbContext _context;
    private readonly ITenantService _tenantService;

    public AddProductAddonCommandHandler(IMenuDigitalDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<Result> Handle(AddProductAddonCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantService.GetTenantId();
        
        if (tenantId == Guid.Empty)
            return Result.Failure(new Error("ProductAddon.TenantRequired", "Acesso Negado: Identificação do restaurante ausente."));

        // Domain-Driven Design: Carregamos a "Raiz de Agregação" (O Produto) para ele mesmo cuidar dos filhos.
        var product = await _context.Products
            .Include(p => p.Addons)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product == null)
            return Result.Failure(new Error("ProductAddon.ProductNotFound", "Produto não encontrado ou não pertence a este restaurante."));

        // É o Produto quem valida as regras de negócio de como adicionar um Addon.
        var result = product.AddAddon(
            request.Name, 
            request.Description, 
            request.Price, 
            request.IsRequired, 
            request.MaxSelection, 
            request.DisplayOrder);

        if (result.IsFailure)
            return Result.Failure(result.Error); // Se feriu a regra de domínio, devolve erro na hora.

        // FORÇAMOS O EF CORE A RECONHECER COMO INSERÇÃO (NOVO)
        // Usamos o DbSet diretamente porque a nossa interface (IMenuDigitalDbContext) não expõe o .Entry()
        _context.ProductAddons.Add(result.Value);

        try
        {
            // O EF Core rastreia as mudanças automaticamente e sabe que precisa fazer um INSERT na tabela ProductAddons
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.FirstOrDefault();
            string entityName = entry?.Metadata.Name ?? "Unknown";
            string entityId = entry?.Property("Id").CurrentValue?.ToString() ?? "Unknown";
            return Result.Failure(new Error("Concurrency", $"Erro de concorrência na entidade {entityName} com ID {entityId}."));
        }

        return Result.Success();
    }
}
