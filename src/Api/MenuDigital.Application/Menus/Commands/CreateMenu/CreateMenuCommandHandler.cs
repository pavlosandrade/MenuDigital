using MediatR;
using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Entities.Menus;
using MenuDigital.Domain.Shared;

namespace MenuDigital.Application.Menus.Commands.CreateMenu;

/// <summary>
/// O Handler que executa a regra de negócio para a criação de um Cardápio.
/// </summary>
public sealed class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, Result<Guid>>
{
    private readonly IMenuDigitalDbContext _context;
    private readonly ITenantService _tenantService; // Injetamos o serviço para ler de qual restaurante é a requisição

    public CreateMenuCommandHandler(IMenuDigitalDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<Result<Guid>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        // 1. Quem está fazendo o pedido?
        var tenantId = _tenantService.GetTenantId();

        if (tenantId == Guid.Empty)
        {
            // Segurança: Se não tem TenantId na requisição, ele não pode criar cardápio de jeito nenhum.
            return Result.Failure<Guid>(new Error(
                "Menu.TenantRequired", 
                "Acesso Negado: Não é possível criar um cardápio sem identificar o restaurante (TenantId ausente)."));
        }

        // 2. Chama a Factory da entidade para criar com validações de domínio
        var menuResult = Menu.Create(tenantId, request.Name, request.StartTime, request.EndTime);

        if (menuResult.IsFailure)
        {
            return Result.Failure<Guid>(menuResult.Error);
        }

        // 3. Adiciona no banco e salva
        _context.Menus.Add(menuResult.Value);
        
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Retorna o ID gerado
        return Result.Success(menuResult.Value.Id);
    }
}
