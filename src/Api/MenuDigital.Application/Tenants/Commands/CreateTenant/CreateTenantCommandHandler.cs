using MediatR;
using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Entities.Tenants;
using MenuDigital.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Application.Tenants.Commands.CreateTenant;

internal sealed class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<Guid>>
{
    private readonly IMenuDigitalDbContext _context;

    public CreateTenantCommandHandler(IMenuDigitalDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // 1. Regra de Negócio Externa: O slug tem que ser único no sistema inteiro
        bool isSlugTaken = await _context.Tenants.AnyAsync(t => t.Slug == request.Slug, cancellationToken);
        if (isSlugTaken)
        {
            return Result.Failure<Guid>(new Error("Tenant.SlugTaken", "Já existe um restaurante utilizando este identificador de URL."));
        }

        // 2. Criação pela Entidade de Domínio Rica
        var tenantResult = Tenant.Create(request.Name, request.Slug);
        
        if (tenantResult.IsFailure)
        {
            return Result.Failure<Guid>(tenantResult.Error);
        }

        var tenant = tenantResult.Value;

        // 3. Persistência
        _context.Tenants.Add(tenant);
        
        // Plus: Já aproveita para criar e vincular as configurações visuais padrões do restaurante
        var settingsResult = RestaurantSettings.CreateDefault(tenant.Id);
        if (settingsResult.IsSuccess)
        {
            _context.RestaurantSettings.Add(settingsResult.Value);
        }

        // Salva tudo de forma transacional
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Retorna Sucesso com o Id gerado
        return Result.Success(tenant.Id);
    }
}
