using MediatR;
using MenuDigital.Application.Interfaces;
using MenuDigital.Domain.Entities.Categories;
using MenuDigital.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace MenuDigital.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly IMenuDigitalDbContext _context;
    private readonly ITenantService _tenantService;

    public CreateCategoryCommandHandler(IMenuDigitalDbContext context, ITenantService tenantService)
    {
        _context = context;
        _tenantService = tenantService;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _tenantService.GetTenantId();

        if (tenantId == Guid.Empty)
        {
            return Result.Failure<Guid>(new Error("Category.TenantRequired", "Acesso Negado: Identificação do restaurante ausente."));
        }

        // 🛡️ SEGURANÇA SÊNIOR (IDOR Prevention)
        // Precisamos garantir que o MenuId passado realmente existe e, mais importante:
        // pertence ao restaurante atual! 
        // Graças ao nosso Global Query Filter configurado na Fase 1, o ".AnyAsync" 
        // abaixo NUNCA vai encontrar um Menu que pertença a outro restaurante.
        var menuExists = await _context.Menus
            .AnyAsync(m => m.Id == request.MenuId, cancellationToken);

        if (!menuExists)
        {
            return Result.Failure<Guid>(new Error(
                "Category.MenuNotFound", 
                "O cardápio informado não foi encontrado ou não pertence a este restaurante."));
        }

        // Se chegou até aqui, é seguro criar a categoria.
        var categoryResult = Category.Create(
            tenantId, 
            request.MenuId, 
            request.Name, 
            request.Description, 
            request.DisplayOrder);

        if (categoryResult.IsFailure)
        {
            return Result.Failure<Guid>(categoryResult.Error);
        }

        _context.Categories.Add(categoryResult.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(categoryResult.Value.Id);
    }
}
